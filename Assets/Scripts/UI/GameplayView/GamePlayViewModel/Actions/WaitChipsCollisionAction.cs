using System.Threading.Tasks;
using Gameplay.ChipsStack;
using UnityEngine;
using Zenject;

namespace UI
{
    public class WaitChipsCollisionAction : BaseGameplayViewModelAction
    {
        [Inject] private ChipsStack _chipsStack;

        private float _maxWaitTime = 5f;

        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            var timer = _maxWaitTime;
            var isNotRest = true;
            while (isNotRest && timer > 0)
            {
                await Task.Yield();
                timer -= Time.deltaTime;

                isNotRest = false;
                foreach (var chip in _chipsStack.Chips)
                {
                    //переделать
                    if (chip.IsRest == false)
                    {
                        isNotRest = true;
                        break;
                    }
                }
            }

        }
    }
}