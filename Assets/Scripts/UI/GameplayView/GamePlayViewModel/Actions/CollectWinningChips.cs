using Gameplay.ChipsStack;
using UnityEngine;
using Zenject;

namespace UI
{
    public class CollectWinningChips : BaseGameplayViewModelAction
    {
        [Inject] private ChipsStack _chipsStack;

        protected override void Execute(GameplayViewModelContext context)
        {
            var winningChips = _chipsStack.Chips.FindAll(c => c.Transform.up.y < c.Transform.position.y);
            
            foreach (var chip in winningChips)
            { 
                Debug.Log($"WIN chip {chip.name}", chip);
            }
        }
    }
}