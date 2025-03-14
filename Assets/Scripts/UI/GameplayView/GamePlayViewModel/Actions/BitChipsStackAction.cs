using Gameplay;
using Gameplay.ChipsStack;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BitChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private ChipsStack _chipsStack;
        [Inject] private CameraController _cameraController;

        protected override void Execute(GameplayViewModelContext context)
        {
            //_chipsStack.Bit(new Vector3(Random.Range(-0.25f, 0.25f), -2f, Random.Range(-0.25f, 0.25f)), ForceMode.Impulse); //хорошо для стопок 6 шт
            
            
            //_chipsStack.Bit(new Vector3(Random.Range(-0.25f, 0.25f), -2f, Random.Range(-0.25f, 0.25f)), ForceMode.Impulse); //хорошо для стопок 6 шт
            
            _chipsStack.Bit(new Vector3(Random.Range(-0.25f, 0.25f), -2.5f, Random.Range(-0.25f, 0.25f)), ForceMode.Impulse);
            
            _cameraController.FollowByChips(_chipsStack.Chips);
        }
    }
}