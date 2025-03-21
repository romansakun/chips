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

        private readonly List<(Chip, ChipDef)> _winningChips = new List<(Chip, ChipDef)>();

        protected override float Score(GameplayViewModelContext context)
        {
            if (context.IsPlayerCannotCollectWinningsChips)
                return 1f;

            _winningChips.Clear();
            foreach (var chipAndDef in  context.HittingChipsAndDefs)
            {
                var chip = chipAndDef.Item1;
                var slopeAngle = Vector3.Angle(chip.Transform.up, Vector3.up);
                var allowedSlopeAngle = _gameDefs.GameplaySettings.AllowedSlopeAngle;
                if (slopeAngle > allowedSlopeAngle && slopeAngle < 180 - allowedSlopeAngle)
                    return 1;

                if (chip.Transform.up.y < chip.Transform.position.y)
                    _winningChips.Add(chipAndDef);
            }

            if (_winningChips.Count > 0)
            {
                context.HitWinningChipsAndDefs.Clear();
                context.HitWinningChipsAndDefs.AddRange(_winningChips);
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}