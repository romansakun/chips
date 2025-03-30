using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class WaitShakingHandsAction :  BaseRockPaperScissorsViewModelAction
    {
        private Sequence _sequence;

        public override async Task ExecuteAsync(RockPaperScissorsViewModelContext context)
        {
            context.PlayerChosenHandVisible.Value = context.RoundPlayers.Contains(PlayerType.MyPlayer);
            context.HandButtonsVisible.Value = false;

            var rockHandSprite = await GetHandSprite(RockPaperScissorsHand.Rock);
            context.PlayerChosenHandSprite.Value = rockHandSprite;
            context.LeftNpcViewBitModelContext.CommunicationSprite.Value = rockHandSprite;
            context.RightNpcViewBitModelContext.CommunicationSprite.Value = rockHandSprite;

            var timerContext = context.TimerViewBitModelContext;
            timerContext.Visible.Value = true;
            context.TitleInfoTextVisible.Value = false;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.OnUpdate(() =>
            {
                if (context.IsDisposed)
                {
                    _sequence.Kill();
                }
            });

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
                        context.LeftNpcViewBitModelContext.CommunicationImageScale.Value = handScale;
                        context.RightNpcViewBitModelContext.CommunicationImageScale.Value = handScale;
                        context.PlayerChosenHandScale.Value = handScale;
                    }, 2f, 1f))
                    .Join(DOTween.To(() => 1f, x =>
                    {
                        var value = 200 / 255f;
                        timerContext.Color.Value = new Color(value, value, value, x);
                    }, 0f, 1f))
                    .JoinCallback(() => timerContext.TimerText.Value = $"{currentTime}");
            }
 
            await _sequence.AsyncWaitForCompletion();

            context.LeftNpcViewBitModelContext.CommunicationImageScale.Value = Vector3.one;
            context.RightNpcViewBitModelContext.CommunicationImageScale.Value = Vector3.one;
            context.PlayerChosenHandScale.Value = Vector3.one;
            timerContext.Visible.Value = false;
        }

    }
}