using System.Threading.Tasks;
using Definitions;
using DG.Tweening;
using Zenject;

namespace UI.SelectingFromAllowedChips
{
    public class AnimateMovingAndRotationAllChipsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] private GameDefs _gameDefs;

        private Sequence _sequence;

        public override async Task ExecuteAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence().SetRecyclable(true);
            _sequence.OnUpdate(() =>
            {
                if (context.IsDisposed)
                    _sequence.Kill();
            });

            var duration = _gameDefs.SelectingChipsForBetSettings.AnimationChipMovingTime;
            var leftPosition = _gameDefs.SelectingChipsForBetSettings.LeftSkippedChipsPosition;
            var leftRotation = _gameDefs.SelectingChipsForBetSettings.LeftSkippedChipsRotation;
            var leftOffset = _gameDefs.SelectingChipsForBetSettings.LeftSkippedChipsPositionOffset;            
            var rightPosition = _gameDefs.SelectingChipsForBetSettings.RightChipsPosition;
            var rightRotation = _gameDefs.SelectingChipsForBetSettings.RightChipsRotation;
            var rightOffset = _gameDefs.SelectingChipsForBetSettings.RightChipsPositionOffset;
            var betPosition = _gameDefs.SelectingChipsForBetSettings.SelectedChipsBetPosition;
            var betRotation = _gameDefs.SelectingChipsForBetSettings.SelectedChipsBetRotation;
            var betOffset = _gameDefs.SelectingChipsForBetSettings.SelectedChipsBetPositionOffset;
            var watchPosition = _gameDefs.SelectingChipsForBetSettings.CurrentWatchingChipPosition;
            var watchRotation = _gameDefs.SelectingChipsForBetSettings.CurrentWatchingChipRotation;

            HideUselessCanvasObjects(context);

            if (context.CurrentWatchingChip != default)
            {
                var chipFacade = context.CurrentWatchingChip.Item1.Facade;
                _sequence
                    .Join(chipFacade.Transform.DOMove(watchPosition, duration).SetRecyclable(true))
                    .Join(chipFacade.Transform.DORotate(watchRotation, duration).SetRecyclable(true));
            }
            for (var i = 0; i < context.RightSideChips.Count; i++)
            {
                var chipFacade = context.RightSideChips[i].Item1.Facade;
                _sequence.Join(chipFacade.Transform.DOMove(rightPosition + rightOffset * i, duration).SetRecyclable(true));
                _sequence.Join(chipFacade.Transform.DORotate(rightRotation, duration).SetRecyclable(true));
            }  
            for (var i = 0; i < context.LeftSideChips.Count; i++)
            {
                var chipFacade = context.LeftSideChips[i].Item1.Facade;
                _sequence.Join(chipFacade.Transform.DOMove(leftPosition + leftOffset * i, duration).SetRecyclable(true));
                _sequence.Join(chipFacade.Transform.DORotate(leftRotation, duration).SetRecyclable(true));
            }
            for (var i = 0; i < context.BetSelectedChips.Count; i++)
            {
                var chipFacade = context.BetSelectedChips[i].Item1.Facade;
                _sequence.Join(chipFacade.Transform.DOMove(betPosition + betOffset * i, duration).SetRecyclable(true));
                _sequence.Join(chipFacade.Transform.DORotate(betRotation, duration).SetRecyclable(true));
            }

            await _sequence.AsyncWaitForCompletion();
        }

        private void HideUselessCanvasObjects(SelectingFromAllowedChipsViewModelContext context)
        {
            context.ShowCurrentWatchingChipCount.Value = false;
        }

    }
}