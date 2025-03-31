using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;

namespace UI
{
    public class PrepareNextRoundAction :  BaseRockPaperScissorsViewModelAction
    {
       
        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            var hasMyPlayer = context.RoundPlayers.Contains(PlayerType.MyPlayer);
            var hasLeftPlayer = context.RoundPlayers.Contains(PlayerType.LeftPlayer);
            var hasRightPlayer = context.RoundPlayers.Contains(PlayerType.RightPlayer);

            await SetPlayer(context.LeftNpcViewBitModelContext, hasLeftPlayer);
            await SetPlayer(context.RightNpcViewBitModelContext, hasRightPlayer);

            context.PlayerInfoTextVisible.Value = hasMyPlayer == false;
            context.TitleInfoTextVisible.Value = hasMyPlayer;

            context.HandButtonsVisible.Value = hasMyPlayer;
            context.PlayerChosenHandVisible.Value = false;
        }

        private async Task SetPlayer(NpcViewBitModelContext playerContext, bool hasPlayerForRound)
        {
            playerContext.VisibleInfoText.Value = hasPlayerForRound == false;
            playerContext.VisibleCommunicationSprite.Value = false;
            await Task.Yield();
        }
    }
}