using System.Collections.Generic;
using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using Managers;
using Zenject;

namespace UI
{
    public class ProcessRoundResultAction :  BaseRockPaperScissorsViewModelAction
    {
        [Inject] private LocalizationManager _localizationManager;

        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            var roundPlayersCount = context.RoundPlayers.Count;
            if (roundPlayersCount > 2)
            {
                ProcessResults(context);
            }
            else if (roundPlayersCount == 2)
            {
                foreach (var playerType in context.RoundPlayers)
                {
                    SetRoundResult(context, playerType);
                }
                var onePlayerResult = context.PlayersResults[context.RoundPlayers[0]];
                var otherPlayerResult = context.PlayersResults[context.RoundPlayers[1]];
                if (onePlayerResult != otherPlayerResult)
                    context.RoundPlayers.Clear();
            }

            context.Players.Sort((p1, p2) => context.PlayersResults[p2].CompareTo(context.PlayersResults[p1]));

            context.LeftNpcViewBitModelContext.VisibleInfoText.Value = true;
            context.RightNpcViewBitModelContext.VisibleInfoText.Value = true;
            context.LeftNpcViewBitModelContext.InfoText.Value = GetPlayerInfoText(context, PlayerType.LeftPlayer);
            context.RightNpcViewBitModelContext.InfoText.Value = GetPlayerInfoText(context, PlayerType.RightPlayer);
            context.PlayerInfoText.Value = GetPlayerInfoText(context, PlayerType.MyPlayer);

            var interval = _gameDefs.RockPaperScissorsSettings.ShowRoundResultsTime;
            var intervalTween = DOTween.To(() => 0f, x => {}, 1f, interval);
            await intervalTween.AsyncWaitForCompletion();
        }

        private string GetPlayerInfoText(RockPaperScissorsViewModelContext context, PlayerType playerType)
        {
            var localizationKey = context.RoundPlayers.Contains(playerType)
                ? _gameDefs.RockPaperScissorsSettings.NeedNextRoundLocalizationKey
                : _gameDefs.RockPaperScissorsSettings.PlayerOrderLocalizationKeys[context.Players.IndexOf(playerType)];
            return _localizationManager.GetText(localizationKey);
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
    }
}