using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MoveSelectedFirstChipToSkippedChipsAction : BaseSelectingFromAllowedChipsViewModelAction
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

            var chip = context.SelectedChips[0];
            context.SelectedChips.RemoveAt(0);
            context.LeftSideChips.Add(chip);
            
            _sequence
                .Append(chip.Facade.Transform.DOMove(new Vector3(-1, 3, -3), .15f))
                .Join(chip.Facade.Transform.DORotate(new Vector3(315, 180, 60), .15f));

            var offset = .025f;
            for (var i = context.LeftSideChips.Count - 1; i >= 1; i--)
            {
                var leftChip = context.LeftSideChips[i];
                _sequence.Join(leftChip.Facade.Transform.DOMove(new Vector3(-1 - offset * (context.LeftSideChips.Count - i), 3, -3), .15f));
            }
            await _sequence.AsyncWaitForCompletion();
        }
    }
}