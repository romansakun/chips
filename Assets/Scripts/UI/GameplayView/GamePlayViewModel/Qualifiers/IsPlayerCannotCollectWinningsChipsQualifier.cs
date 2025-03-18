using System.Collections.Generic;
using Gameplay;
using Gameplay.Chips;
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

        private readonly List<Chip> _winningChips = new List<Chip>();
        
        protected override float Score(GameplayViewModelContext context)
        {
            if (context.IsPlayerCannotCollectWinningsChips)
                return 1f;

            _winningChips.Clear();
            //var sqrAllowedScatterRadius = _gameRules.AllowedScatterRadius * _gameRules.AllowedScatterRadius;
            foreach (var chip in  _chipsStack.Chips)
            {
                var slopeAngle = Vector3.Angle(chip.Transform.up, Vector3.up);
                if (slopeAngle > _gameRules.AllowedSlopeAngle && slopeAngle < 180 - _gameRules.AllowedSlopeAngle)
                    return 1;
                
                if (chip.Transform.up.y < chip.Transform.position.y)
                    _winningChips.Add(chip);
            }
            
            if (_winningChips.Count > 0)
            {
                // _cameraController.CancelFollowing();
                // _cameraController.FollowByChips(_winningChips);
                context.HitWinningChips.Clear();
                context.HitWinningChips.AddRange(_winningChips);
                context.IsHitSuccess.Value = true;
                return 0;
            }
            else
            {
                context.IsHitFailed.Value = true;
                return 1;
            }
        }
    }
}