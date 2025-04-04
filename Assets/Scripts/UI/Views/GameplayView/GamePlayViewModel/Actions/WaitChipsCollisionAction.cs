using System.Threading.Tasks;
using Definitions;
using Gameplay;
using Installers;
using UnityEngine;
using Zenject;

namespace UI.Gameplay
{
    public class WaitChipsCollisionAction : BaseGameplayViewModelAction
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private CameraController _cameraController;

        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            context.HitTimerContext.Visible.Value = false;
            context.HitTimerContext.Color.Value = _colorsSettings.WhiteTextColor;
            var allowedScatterRadius = _gameDefs.GameplaySettings.AllowedScatterRadius;
            var sqrAllowedScatterRadius = allowedScatterRadius * allowedScatterRadius;
            var waitingTime = _gameDefs.GameplaySettings.MaxTimeToWaitHitResult;
            while (waitingTime > 0 && CanWait(context))
            {
                var canFinishedWaiting = true;
                foreach (var chip in context.HittingChips)
                {
                    var position = chip.Facade.Transform.position;
                    position.y = 0;
                    if (position.sqrMagnitude > sqrAllowedScatterRadius)
                    {
                        context.IsPlayerCannotCollectWinningsChips = true;
                        break;
                    }
                    if (chip.Facade.Rigidbody.isKinematic == false)
                    {
                        canFinishedWaiting = false;
                    }
                }

                ProcessHitTimer(context, waitingTime);

                if (canFinishedWaiting)
                    waitingTime = 0;
                else
                    waitingTime -= Time.deltaTime;

                await Task.Yield();
            }

            _cameraController.CancelFollowing();
            context.HitTimerContext.Visible.Value = false;
        }

        private void ProcessHitTimer(GameplayViewModelContext context, float waitingTime)
        {
            if (context.HitTimerContext.Visible.Value == false)
            {
                var timeDiff = _gameDefs.GameplaySettings.MaxTimeToWaitHitResult - waitingTime;
                var needTimer = timeDiff > _gameDefs.GameplaySettings.FirstTimeToWaitHitResult;
                if (needTimer)
                    context.HitTimerContext.Visible.Value = true;
                else
                    return;
            }

            var value = Mathf.CeilToInt(waitingTime).ToString();
            context.HitTimerContext.TimerText.Value = value;
        }

        private static bool CanWait(GameplayViewModelContext context)
        {
            return context.IsDisposed == false && 
                   context.IsPlayerCannotCollectWinningsChips == false;
        }
    }
}