using Definitions;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MovingAndRotationAllChipsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] private GameDefs _gameDefs;

        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
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

            context.CurrentWatchingChip.Item1.Facade.Transform.SetPositionAndRotation(watchPosition, Quaternion.Euler(watchRotation));

            for (var i = 0; i < context.RightSideChips.Count; i++)
            {
                var chipTransform = context.RightSideChips[i].Item1.Facade.Transform;
                chipTransform.SetPositionAndRotation(rightPosition + rightOffset * i, Quaternion.Euler(rightRotation));
            }  
            for (var i = 0; i < context.LeftSideChips.Count; i++)
            {
                var chipTransform = context.LeftSideChips[i].Item1.Facade.Transform;
                chipTransform.SetPositionAndRotation(leftPosition + leftOffset * i, Quaternion.Euler(leftRotation));
            }
            for (var i = 0; i < context.BetSelectedChips.Count; i++)
            {
                var chipTransform = context.BetSelectedChips[i].Item1.Facade.Transform;
                chipTransform.SetPositionAndRotation(betPosition + betOffset * i, Quaternion.Euler(betRotation));
            }
        }
    }
}