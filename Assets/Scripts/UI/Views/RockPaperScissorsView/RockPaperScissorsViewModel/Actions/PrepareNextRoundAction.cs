using System.Threading.Tasks;
using Definitions;

namespace UI.RockPaperScissors
{
    public class PrepareNextRoundAction :  BaseRockPaperScissorsViewModelAction
    {
       
        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            var hasMyPlayer = context.RoundPlayers.Contains(PlayerType.MyPlayer);
            var hasLeftPlayer = context.RoundPlayers.Contains(PlayerType.LeftPlayer);
            var hasRightPlayer = context.RoundPlayers.Contains(PlayerType.RightPlayer);

            await SetPlayer(context.LeftNpcViewComponentModelContext, hasLeftPlayer);
            await SetPlayer(context.RightNpcViewComponentModelContext, hasRightPlayer);

            context.PlayerInfoTextVisible.Value = hasMyPlayer == false;
            context.TitleInfoTextVisible.Value = hasMyPlayer;

            context.HandButtonsVisible.Value = hasMyPlayer;
            context.PlayerChosenHandVisible.Value = false;
        }

        private async Task SetPlayer(NpcViewPartModelContext playerContext, bool hasPlayerForRound)
        {
            playerContext.VisibleInfoText.Value = hasPlayerForRound == false;
            playerContext.VisibleCommunicationSprite.Value = false;
            await Task.Yield();
        }
    }
}