using System.Threading.Tasks;
using Definitions;
using Gameplay;
using UnityEngine;
using Zenject;

namespace UI
{
    public class WaitChipsCollisionAction : BaseGameplayViewModelAction
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private ChipsStack _chipsStack;

        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            context.ShowHitTimer.Value = true;

            var allowedScatterRadius = _gameDefs.GameplaySettings.AllowedScatterRadius;
            var sqrAllowedScatterRadius = allowedScatterRadius * allowedScatterRadius;
            var waitingTime = _gameDefs.GameplaySettings.MaxTimeToWaitHitResult;
            while (waitingTime > 0 && CanWait(context))
            {
                var canFinishedWaiting = true;
                foreach (var chip in _chipsStack.Chips)
                {
                    // there was a step over the line => need repeat hit
                    var position = chip.Transform.position;
                    position.y = 0;
                    if (position.sqrMagnitude > sqrAllowedScatterRadius)
                    {
                        context.IsPlayerCannotCollectWinningsChips = true;
                        break;
                    }
                    // the chip is not a rest => keep waiting
                    if (chip.Rigidbody.isKinematic == false)
                    {
                        canFinishedWaiting = false;
                    }
                }

                context.HitTimer.Value = waitingTime;

                if (canFinishedWaiting)
                    waitingTime = 0;
                else
                    waitingTime -= Time.deltaTime;

                await Task.Yield();
            }

            context.ShowHitTimer.Value = false;
        }

        private static bool CanWait(GameplayViewModelContext context)
        {
            return context.IsDisposed == false && 
                   context.IsPlayerCannotCollectWinningsChips == false;
        }
    }
}