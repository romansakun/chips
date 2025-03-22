using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MoveSelectFirstRightAllowedChipToBetAction : BaseSelectingFromAllowedChipsViewModelAction
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

            var chip = context.RightSideChips[0];
            context.RightSideChips.RemoveAt(0);
            context.SelectedChips.Insert(0, chip);

            _sequence
                .Append(chip.Facade.Transform.DOMove(new Vector3(0, 3, 0), .15f))
                .Join(chip.Facade.Transform.DORotate(new Vector3(360, 180, 150), .15f));

            var offset = .025f;
            for (var i = 1; i < context.SelectedChips.Count; i++)
            {
                var selectedChip = context.SelectedChips[i];
                _sequence.Join(selectedChip.Facade.Transform.DOMove(new Vector3(offset * i, 3, 0), .15f));
            }

            await _sequence.AsyncWaitForCompletion();
        }
    }
}