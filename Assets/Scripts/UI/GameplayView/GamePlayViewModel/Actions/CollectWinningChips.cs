using System.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using Gameplay.Chips;
using Zenject;

namespace UI
{
    public class CollectWinningChips : BaseGameplayViewModelAction
    {
        [Inject] private ChipsStack _chipsStack;
        [Inject] private CameraController _cameraController;
        [Inject] private GameplayObjectsHolder _destinations;

        private Sequence _collectionSequence;
        
        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            //TODO засчитать игроку победные фишки

            _collectionSequence?.Kill();
            _collectionSequence = DOTween.Sequence();
            _collectionSequence.OnUpdate(() =>
            {
                if (context.IsDisposed)
                    _collectionSequence.Kill();
            });
            _cameraController.CancelFollowing();
            var destination = _destinations.MyPlayerChipDestination.position;
            foreach (var chip in context.HitWinningChips)
            {
                _collectionSequence
                    .AppendCallback(() => chip.Rigidbody.isKinematic = true)
                    .Append(chip.Transform
                        .DOMove(destination, 0.5f)
                        .SetEase(Ease.InQuint));
            }

            await _collectionSequence.AsyncWaitForCompletion();
        }

    }
}