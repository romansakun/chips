using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using Extensions;
using Managers;
using Zenject;

namespace UI.RockPaperScissors
{
    public class ProcessRoundResultAction :  BaseRockPaperScissorsViewModelAction
    {
        [Inject] private LocalizationManager _localizationManager;
        [Inject] private SignalBus _signalBus;

        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            var roundPlayersCount = context.RoundPlayers.Count;
            if (roundPlayersCount > 2)
            {
                ProcessResults(context);
            }
            else
            {
                foreach (var playerType in context.RoundPlayers)
                {
                    SetRoundResult(context, playerType);
                }
                var onePlayerResult = context.PlayersResults[context.RoundPlayers[0]];
                var otherPlayerResult = context.PlayersResults[context.RoundPlayers[1]];
                if (onePlayerResult != otherPlayerResult)
                {
                    context.RoundPlayers.Clear();
                    SetPlayersMoveOrder(context);
                }
            }

            context.LeftNpcViewComponentModelContext.VisibleInfoText.Value = true;
            context.RightNpcViewComponentModelContext.VisibleInfoText.Value = true;
            context.LeftNpcViewComponentModelContext.InfoText.Value = GetPlayerInfoText(context, PlayerType.LeftPlayer);
            context.RightNpcViewComponentModelContext.InfoText.Value = GetPlayerInfoText(context, PlayerType.RightPlayer);
            context.PlayerInfoText.Value = GetPlayerInfoText(context, PlayerType.MyPlayer);

            var interval = _gameDefs.RockPaperScissorsSettings.ShowRoundResultsTime;
            var intervalTween = DOTween.To(() => 0f, x => {}, 1f, interval);
            await intervalTween.AsyncWaitForCompletion();

            //todo: it need to replace to next action
            if (context.RoundPlayers.Count == 0)
            {
                _signalBus.Fire<PlayersMoveOrderSetSignal>();
            }
        }

        private string GetPlayerInfoText(RockPaperScissorsViewModelContext context, PlayerType playerType)
        {
            if (context.RoundPlayers.Contains(playerType))
                return _localizationManager.GetText(_gameDefs.RockPaperScissorsSettings.NeedNextRoundLocalizationKey);

            var loserPair = context.PlayersResults.GetMinByValue();
            if (loserPair.Key == playerType)
                return _localizationManager.GetText(_gameDefs.RockPaperScissorsSettings.LoserLocalizationKey);

            var winnerPair = context.PlayersResults.GetMaxByValue();
            if (winnerPair.Key == playerType)
                return _localizationManager.GetText(_gameDefs.RockPaperScissorsSettings.WinnerLocalizationKey);

            return _localizationManager.GetText(_gameDefs.RockPaperScissorsSettings.SecondPlayerLocalizationKey);
        }

        private void ProcessResults(RockPaperScissorsViewModelContext context)
        {
            foreach (var playerType in context.RoundPlayers)
            {
                SetRoundResult(context, playerType);
            }
            var resultsCounts = new Dictionary<int, int>();
            foreach (var pair in context.PlayersResults)
            {
                if (resultsCounts.ContainsKey(pair.Value))
                    resultsCounts[pair.Value]++;
                else
                    resultsCounts[pair.Value] = 1;
            }
            context.RoundPlayers.RemoveAll(p=> resultsCounts[context.PlayersResults[p]] < 2);
        }

        private void SetRoundResult(RockPaperScissorsViewModelContext context, PlayerType playerType)
        {
            var result = 0;
            var playerHand = context.RoundPlayersHands[playerType];
            foreach (var pair in context.RoundPlayersHands)
            {
                if (pair.Key == playerType)
                    continue;

                var otherHand = pair.Value;
                if (playerHand == otherHand.Opposite()) 
                    result++;

                if (playerHand.Opposite() == otherHand)
                    result--;
            }
            context.PlayersResults[playerType] = result;
        }

        private void SetPlayersMoveOrder(RockPaperScissorsViewModelContext context)
        {
            var playersResults = new Dictionary<PlayerType, int>(context.PlayersResults);
            var pair = playersResults.GetMaxByValue();
            playersResults.Remove(pair.Key);
            context.Shared.FirstMovePlayer = context.Shared.Players.Find(p => p.Type == pair.Key);
            var player = context.Shared.FirstMovePlayer;
            while (playersResults.Count > 0)
            {
                pair = playersResults.GetMaxByValue();
                playersResults.Remove(pair.Key);
                player.NextPlayerTypeInTurn = pair.Key;
                player = context.Shared.Players.Find(p => p.Type == pair.Key);
            }
            player.NextPlayerTypeInTurn = context.Shared.FirstMovePlayer.Type;
        }
    }
}