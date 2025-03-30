using System.Threading.Tasks;
using Definitions;
using Gameplay.Battle;
using Managers;
using Zenject;

namespace UI
{
    public class PrepareAction : BaseRockPaperScissorsViewModelAction
    {
        [Inject] private BattleController _battleController;
        [Inject] private LocalizationManager _localizationManager;

        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            context.Players.Add(PlayerType.MyPlayer);
            var leftPlayerId = _battleController.Context.LeftPlayer;
            if (string.IsNullOrEmpty(leftPlayerId) == false)
            {
                context.Players.Add(PlayerType.LeftPlayer);
                context.LeftNpcViewBitModelContext.Visible.Value = true;
                context.LeftNpcViewBitModelContext.AvatarSprite.Value = await GetAvatarSprite(leftPlayerId);
            }
            var rightPlayerId = _battleController.Context.RightPlayer;
            if (string.IsNullOrEmpty(rightPlayerId) == false)
            {
                context.Players.Add(PlayerType.RightPlayer);
                context.RightNpcViewBitModelContext.Visible.Value = true;
                context.RightNpcViewBitModelContext.AvatarSprite.Value = await GetAvatarSprite(rightPlayerId);
            }
            context.RoundPlayers.AddRange(context.Players);
            
            context.RockButtonSprite.Value = await GetHandSprite(RockPaperScissorsHand.Rock);
            context.PaperButtonSprite.Value = await GetHandSprite(RockPaperScissorsHand.Paper);
            context.ScissorsButtonSprite.Value = await GetHandSprite(RockPaperScissorsHand.Scissors);
            context.PlayerInfoTextVisible.Value = false;
            context.HandButtonsVisible.Value = true;
            context.TitleInfoTextVisible.Value = true;
            context.TitleInfoText.Value = _localizationManager.GetText(_gameDefs.RockPaperScissorsSettings.SelectHandTitleLocalizationKey);
        }
    }
}