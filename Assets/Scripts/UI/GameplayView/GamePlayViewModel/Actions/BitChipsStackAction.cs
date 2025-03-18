using Gameplay;
using Managers;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BitChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private AudioManager _audioManager;
        [Inject] private ChipsStack _chipsStack;
        [Inject] private CameraController _cameraController;

        protected override void Execute(GameplayViewModelContext context)
        {
            //_chipsStack.Bit(new Vector3(Random.Range(-0.25f, 0.25f), -2f, Random.Range(-0.25f, 0.25f)), ForceMode.Impulse); //хорошо для стопок 6 шт

            context.IsPlayerCannotCollectWinningsChips = false;
            _audioManager.PrepareForGroundChipsHitSound();

            
            _chipsStack.Bit(new Vector3(Random.Range(-0.25f, 0.25f), -2.5f, Random.Range(-0.25f, 0.25f)), ForceMode.Impulse);

            _cameraController.FollowByChips(_chipsStack.Chips);
        }
    }
}