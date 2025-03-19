using System.Collections.Generic;
using Definitions;
using Gameplay;
using Gameplay.Chips;
using UnityEngine;
using Zenject;

namespace UI
{
    public class IsPlayerCannotCollectWinningsChipsQualifier : BaseGameplayViewModelQualifier
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private CameraController _cameraController;
        [Inject] private ChipsStack _chipsStack;

        private readonly List<Chip> _winningChips = new List<Chip>();
        
        protected override float Score(GameplayViewModelContext context)
        {
            if (context.IsPlayerCannotCollectWinningsChips)
                return 1f;

            _winningChips.Clear();
            foreach (var chip in  _chipsStack.Chips)
            {
                var slopeAngle = Vector3.Angle(chip.Transform.up, Vector3.up);
                var allowedSlopeAngle = _gameDefs.GameplaySettings.AllowedSlopeAngle;
                if (slopeAngle > allowedSlopeAngle && slopeAngle < 180 - allowedSlopeAngle)
                    return 1;

                if (chip.Transform.up.y < chip.Transform.position.y)
                    _winningChips.Add(chip);
            }

            if (_winningChips.Count > 0)
            {
                context.HitWinningChips.Clear();
                context.HitWinningChips.AddRange(_winningChips);
                context.IsHitSuccess.Value = true; 
                return 0;
            }
            else
            {
                context.IsHitFailed.Value = true; //вынести в отдельный экшн
                return 1;
            }
        }
    }
}