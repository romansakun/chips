using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using Installers;
using UnityEngine;
using Zenject;

namespace UI.RockPaperScissors
{
    public class WaitShakingHandsAction :  BaseRockPaperScissorsViewModelAction
    {
        [Inject] private ColorsSettings _colorsSettings;

        private Sequence _sequence;

        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            context.PlayerChosenHandVisible.Value = context.RoundPlayers.Contains(PlayerType.MyPlayer);
            context.HandButtonsVisible.Value = false;

            var rockHandSprite = await GetHandSprite(RockPaperScissorsHand.Rock);
            context.PlayerChosenHandSprite.Value = rockHandSprite;
            context.LeftNpcViewComponentModelContext.VisibleCommunicationSprite.Value = context.RoundPlayers.Contains(PlayerType.LeftPlayer);
            context.RightNpcViewComponentModelContext.VisibleCommunicationSprite.Value = context.RoundPlayers.Contains(PlayerType.RightPlayer);
            context.LeftNpcViewComponentModelContext.CommunicationSprite.Value = rockHandSprite;
            context.RightNpcViewComponentModelContext.CommunicationSprite.Value = rockHandSprite;

            var timerContext = context.TimerViewPartModelContext;
            timerContext.Visible.Value = true;
            context.TitleInfoTextVisible.Value = false;

            _sequence?.Kill();
            _sequence = DOTween.Sequence().SetRecyclable(true);
            _sequence.OnUpdate(() =>
            {
                if (context.IsDisposed)
                {
                    _sequence.Kill();
                }
            });

            //todo: it's not nice, you should think about it some more
            var seconds = _gameDefs.RockPaperScissorsSettings.ShakingHandsTime;
            while (seconds > 0)
            {
                var currentTime = seconds;
                seconds -= 1;
                _sequence
                    .Append(DOTween.To(() => 1f, x =>
                    {
                        timerContext.Scale.Value = new Vector3(x, x, x);
                        x -= 1f;
                        if (x > 0.5f) x = 0.5f - (x - 0.5f);
                        x /= 2f;
                        var handScaleValue = x + 1f;
                        var handScale = new Vector3(handScaleValue, handScaleValue, handScaleValue);
                        context.LeftNpcViewComponentModelContext.CommunicationImageScale.Value = handScale;
                        context.RightNpcViewComponentModelContext.CommunicationImageScale.Value = handScale;
                        context.PlayerChosenHandScale.Value = handScale;
                    }, 2f, 1f).SetRecyclable(true))
                    .Join(DOTween.To(() => 1f, x =>
                    {
                        var color = _colorsSettings.TimerColor;
                        color.a = x;
                        timerContext.Color.Value = color;
                    }, 0f, 1f).SetRecyclable(true))
                    .JoinCallback(() => timerContext.TimerText.Value = $"{currentTime}");
            }
 
            await _sequence.AsyncWaitForCompletion();

            context.LeftNpcViewComponentModelContext.CommunicationImageScale.Value = Vector3.one;
            context.RightNpcViewComponentModelContext.CommunicationImageScale.Value = Vector3.one;
            context.PlayerChosenHandScale.Value = Vector3.one;
            timerContext.Visible.Value = false;
        }

    }
}