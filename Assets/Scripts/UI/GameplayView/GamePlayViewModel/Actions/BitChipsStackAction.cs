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
        [Inject] private PlayerContextRepository _playerContext;

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
                _playerContext.RandomRange(-0.25f, 0.25f),
                -2.5f,
                _playerContext.RandomRange(-0.25f, 0.25f));
            var torque = new Vector3(
                _playerContext.RandomRange(-.05f, .05f),
                _playerContext.RandomRange(-.5f, .5f),
                _playerContext.RandomRange(-.05f, .05f));

            Debug.Log($"bitForce: {bitForce} torque: {torque}");

            foreach (var chip in chips)
            {
                chip
                    .PrepareForHit()
                    .AddForce(bitForce, forceMode)
                    .AddTorque(torque, ForceMode.Impulse);
            }
        }
    }
}