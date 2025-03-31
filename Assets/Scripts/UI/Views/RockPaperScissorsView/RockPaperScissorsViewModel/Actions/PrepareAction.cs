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
            foreach (var player in context.Shared.Players)
            {
                context.RoundPlayers.Add(player.Type);

                if (player.Type == PlayerType.LeftPlayer)
                {
                    context.LeftNpcViewBitModelContext.Visible.Value = true;
                    context.LeftNpcViewBitModelContext.AvatarSprite.Value = await GetAvatarSprite(player.Id);
                }
                else if (player.Type == PlayerType.RightPlayer)
                {
                    context.RightNpcViewBitModelContext.Visible.Value = true;
                    context.RightNpcViewBitModelContext.AvatarSprite.Value = await GetAvatarSprite(player.Id);
                }
            }

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