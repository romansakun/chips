using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using Gameplay;
using Gameplay.Battle;
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

            var hittingPlayer = context.Players[context.HittingPlayerIndex];
            var chipsDestination = Vector3.positiveInfinity;
            switch (hittingPlayer.PlayerType)
            {
                case PlayerType.User: chipsDestination = _destinations.MyPlayerChipDestination.position; break;
                case PlayerType.LeftNpc: chipsDestination = _destinations.LeftPlayerChipDestination.position; break;
                case PlayerType.RightNpc: chipsDestination = _destinations.RightPlayerChipDestination.position; break;
            }

            foreach (var chipAndDef in context.HitWinningChipsAndDefs)
            {
                var chip = chipAndDef.Item1;
                var chipDef = chipAndDef.Item2;

                context.HittingChipsAndDefs.Remove(chipAndDef);
                hittingPlayer.WinningChips.Add(chipDef.Id);

                _collectionSequence
                    .AppendCallback(() => chip.Rigidbody.isKinematic = true)
                    .Append(chip.Transform
                        .DOMove(chipsDestination, _gameDefs.GameplaySettings.TimeToMoveWinningChipToPlayer)
                        .SetEase(Ease.InQuint));
            }

            await _collectionSequence.AsyncWaitForCompletion();
        }

    }
}