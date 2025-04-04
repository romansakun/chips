using System.Threading.Tasks;
using Definitions;

namespace UI.RockPaperScissors
{
    public class SetHandsAction :  BaseRockPaperScissorsViewModelAction
    {
        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            if (context.RoundPlayers.Contains(PlayerType.MyPlayer))
            {
                var playerHand = context.RoundPlayersHands[PlayerType.MyPlayer];
                context.PlayerChosenHandSprite.Value = await GetHandSprite(playerHand);
                context.PlayerChosenHandVisible.Value = true;
                context.HandButtonsVisible.Value = false;
            }
            if (context.RoundPlayers.Contains(PlayerType.LeftPlayer))
            {
                var leftNpcHand = RockPaperScissorsHandExt.Random();
                context.RoundPlayersHands[PlayerType.LeftPlayer] = leftNpcHand;
                context.LeftNpcViewComponentModelContext.CommunicationSprite.Value = await GetHandSprite(leftNpcHand);
            }
            if (context.RoundPlayers.Contains(PlayerType.RightPlayer))
            {
                var rightNpcHand = RockPaperScissorsHandExt.Random();
                context.RoundPlayersHands[PlayerType.RightPlayer] = rightNpcHand;
                context.RightNpcViewComponentModelContext.CommunicationSprite.Value = await GetHandSprite(rightNpcHand);
            }

            foreach (var player in context.Shared.Players)
            {
                if (context.RoundPlayers.Contains(player.Type) == false)
                    context.RoundPlayersHands.Remove(player.Type);
            }
        }

    }
}