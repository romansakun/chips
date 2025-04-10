using System.Collections.Generic;
using Definitions;
using Gameplay.Chips;
using UnityEngine;
using Zenject;

namespace UI.Gameplay
{
    public class IsPlayerCannotCollectWinningsChipsQualifier : BaseGameplayViewModelQualifier
    {
        [Inject] private GameDefs _gameDefs;

        private readonly List<(Chip, ChipDef)> _winningChips = new();

        protected override float Score(GameplayViewModelContext context)
        {
            if (context.IsPlayerCannotCollectWinningsChips)
                return 1f;

            _winningChips.Clear();
            foreach (var chipAndDef in  context.HittingChipsAndDefs)
            {
                var chip = chipAndDef.Item1;
                var chipTransform = chip.Facade.Transform;
                var slopeAngle = Vector3.Angle(chipTransform.up, Vector3.up);
                var allowedSlopeAngle = _gameDefs.GameplaySettings.AllowedSlopeAngle;

                //todo replace it to special class helper
                if (slopeAngle > allowedSlopeAngle && slopeAngle < 180 - allowedSlopeAngle)
                    return 1;

                if (chipTransform.up.y < chipTransform.position.y)
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