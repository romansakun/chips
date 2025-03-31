using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using Zenject;

namespace UI
{
    public class CollectWinningChips : BaseGameplayViewModelAction
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private CameraController _cameraController;
        [Inject] private GameplayObjectsHolder _destinations;

        private Sequence _collectionSequence;

        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            _collectionSequence?.Kill();
            _collectionSequence = DOTween.Sequence();
            _collectionSequence.OnUpdate(() =>
            {
                if (context.IsDisposed)
                    _collectionSequence.Kill();
            });
            _cameraController.CancelFollowing();

            var hittingPlayer = context.HittingPlayer;
            var chipsDestination = Vector3.positiveInfinity;
            switch (hittingPlayer.Type)
            {
                case PlayerType.MyPlayer: chipsDestination = _destinations.MyPlayerChipDestination.position; break;
                case PlayerType.LeftPlayer: chipsDestination = _destinations.LeftPlayerChipDestination.position; break;
                case PlayerType.RightPlayer: chipsDestination = _destinations.RightPlayerChipDestination.position; break;
            }

            foreach (var chipAndDef in context.HitWinningChipsAndDefs)
            {
                var chip = chipAndDef.Item1;
                var chipDef = chipAndDef.Item2;

                context.HittingChipsAndDefs.Remove(chipAndDef);
                hittingPlayer.WinningChips.Add(chipDef);

                _collectionSequence
                    .AppendCallback(() => chip.Facade.Rigidbody.isKinematic = true)
                    .Append(chip.Facade.Transform
                        .DOMove(chipsDestination, _gameDefs.GameplaySettings.TimeToMoveWinningChipToPlayer)
                        .SetEase(Ease.InQuint));
            }

            await _collectionSequence.AsyncWaitForCompletion();
        }

    }
}