using Gameplay;
using Installers;
using UnityEngine;
using Zenject;

namespace UI
{
    public class IsPlayerCannotCollectWinningsChipsQualifier : BaseGameplayViewModelQualifier
    {
        [Inject] private GameRules _gameRules;
        [Inject] private CameraController _cameraController;
        [Inject] private ChipsStack _chipsStack;

        protected override float Score(GameplayViewModelContext context)
        {
            if (context.IsPlayerCannotCollectWinningsChips)
                return 1f;

            //var sqrAllowedScatterRadius = _gameRules.AllowedScatterRadius * _gameRules.AllowedScatterRadius;
            foreach (var chip in  _chipsStack.Chips)
            {
                var slopeAngle = Vector3.Angle(chip.Transform.up, Vector3.up);
                if (slopeAngle > _gameRules.AllowedSlopeAngle && slopeAngle < 180 - _gameRules.AllowedSlopeAngle)
                    return 1;
            }

            return 0;
        }
    }
}