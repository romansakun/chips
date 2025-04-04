using System.Threading.Tasks;
using Definitions;
using Managers;
using Zenject;

namespace UI.RockPaperScissors
{
    public class PrepareAction : BaseRockPaperScissorsViewModelAction
    {
        [Inject] private LocalizationManager _localizationManager;

        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            foreach (var player in context.Shared.Players)
            {
                context.RoundPlayers.Add(player.Type);

                if (player.Type == PlayerType.LeftPlayer)
                {
                    context.LeftNpcViewComponentModelContext.AvatarSprite.Value = await GetAvatarSprite(player.Id);
                    context.LeftNpcViewComponentModelContext.Visible.Value = true;
                }
                else if (player.Type == PlayerType.RightPlayer)
                {
                    context.RightNpcViewComponentModelContext.AvatarSprite.Value = await GetAvatarSprite(player.Id);
                    context.RightNpcViewComponentModelContext.Visible.Value = true;
                }
            }

            context.RockButtonSprite.Value = await GetHandSprite(RockPaperScissorsHand.Rock);
            context.PaperButtonSprite.Value = await GetHandSprite(RockPaperScissorsHand.Paper);
            context.ScissorsButtonSprite.Value = await GetHandSprite(RockPaperScissorsHand.Scissors);
            context.PlayerInfoTextVisible.Value = false;
            context.HandButtonsVisible.Value = true;
            context.TitleInfoTextVisible.Value = true;
            var localizationKey = _gameDefs.RockPaperScissorsSettings.SelectHandTitleLocalizationKey;
            context.TitleInfoText.Value = _localizationManager.GetText(localizationKey);
        }
    }
}