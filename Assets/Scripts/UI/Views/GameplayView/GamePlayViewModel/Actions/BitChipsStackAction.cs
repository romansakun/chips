using Gameplay;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace UI.Gameplay
{
    public class BitChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private AudioManager _audioManager;
        [Inject] private CameraController _cameraController;
        [Inject] private UserContextRepository _userContext;

        protected override void Execute(GameplayViewModelContext context)
        {
            _audioManager.PrepareForGroundChipsHitSound();

            context.IsPlayerCannotCollectWinningsChips = false;
            Bit(context);

            _cameraController.FollowByChips(context.HittingChips);
        }

        private void Bit(GameplayViewModelContext context, ForceMode forceMode = ForceMode.Impulse)
        {
            var bitForce = context.PlayerHitForce;
            var bitTorque = context.PlayerHitTorque;
            foreach (var chip in context.HittingChips)
            {
                var chipFacade = chip.Facade;
                chipFacade.ResetRestFramesCount();
                chipFacade.GameObject.SetActive(true);
                chipFacade.Rigidbody.isKinematic = false;
                chipFacade.Rigidbody.AddForce(bitForce, forceMode);
                chipFacade.Rigidbody.AddTorque(bitTorque, forceMode);
            }

            Debug.Log($"bitForce: {bitForce} torque: {bitTorque}");
        }
    }
}