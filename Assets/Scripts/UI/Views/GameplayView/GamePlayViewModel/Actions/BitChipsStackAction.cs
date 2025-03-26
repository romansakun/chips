using System.Collections.Generic;
using Gameplay;
using Gameplay.Chips;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BitChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private AudioManager _audioManager;
        [Inject] private CameraController _cameraController;
        [Inject] private UserContextRepository _userContext;

        protected override void Execute(GameplayViewModelContext context)
        {
            //_chipsStack.Bit(new Vector3(Random.Range(-0.25f, 0.25f), -2f, Random.Range(-0.25f, 0.25f)), ForceMode.Impulse); //хорошо для стопок 6 шт

            context.IsPlayerCannotCollectWinningsChips = false;
            _audioManager.PrepareForGroundChipsHitSound();

            Bit(context.HittingChips);

            _cameraController.FollowByChips(context.HittingChips);
        }

        private void Bit(List<Chip> chips, ForceMode forceMode = ForceMode.Impulse)
        {
            // // Вектор отскока (нормаль к поверхности столкновения)
            // Vector3 bounceDirection = collision.contacts[0].normal;
            //
            // // Добавляем случайный боковой вектор для изменения угла
            // Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * bounceDirection;
            //
            // // Применяем силу отскока
            // _rigidbody.AddForce(randomDirection * 5, ForceMode.Impulse);
            //
            // // Добавляем случайный крутящий момент для вращения
            var bitForce = new Vector3(
                _userContext.RandomRange(-0.25f, 0.25f),
                -2.5f,
                _userContext.RandomRange(-0.25f, 0.25f));
            var torque = new Vector3(
                _userContext.RandomRange(-.05f, .05f),
                _userContext.RandomRange(-.5f, .5f),
                _userContext.RandomRange(-.05f, .05f));

            Debug.Log($"bitForce: {bitForce} torque: {torque}");

            foreach (var chip in chips)
            {
                var chipFacade = chip.Facade;
                chipFacade.ResetRestFramesCount();
                chipFacade.GameObject.SetActive(true);
                chipFacade.Rigidbody.isKinematic = false;
                chipFacade.Rigidbody.AddForce(bitForce, forceMode);
                chipFacade.Rigidbody.AddTorque(torque, forceMode);
            }
        }
    }
}