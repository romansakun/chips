using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MoveLastSkippedChipToRightAllowedChipsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        private Sequence _sequence;
        
        public override async Task ExecuteAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.OnUpdate(() =>
            {
                if (context.IsDisposed)
                    _sequence.Kill();
            });

            var chip = context.LeftSideChips[^1];
            context.LeftSideChips.Remove(chip);
            context.RightSideChips.Insert(0, chip);

            _sequence
                .Append(chip.Facade.Transform.DOMove(new Vector3(0, 3, -3), .15f))
                .Join(chip.Facade.Transform.DORotate(new Vector3(315, 180, 150), .15f));

            var offset = .025f;
            for (var i = 1; i < context.RightSideChips.Count; i++)
            {
                var rightChip = context.RightSideChips[i];
                _sequence.Join(rightChip.Facade.Transform.DOMove(new Vector3( offset * i, 3, -3), .15f));
            }
            for (var i = 0; i < context.LeftSideChips.Count; i++)
            {
                var leftChip = context.LeftSideChips[i];
                _sequence.Join(leftChip.Facade.Transform.DOMove(new Vector3(-1 - offset * (context.LeftSideChips.Count - i), 3, -3), .15f));
            }
            await _sequence.AsyncWaitForCompletion();
        }
    }
}