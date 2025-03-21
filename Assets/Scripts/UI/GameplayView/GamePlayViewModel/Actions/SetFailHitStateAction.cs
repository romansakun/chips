using System.Threading.Tasks;
using Definitions;
using Gameplay;
using Installers;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SetFailHitStateAction : BaseGameplayViewModelAction
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private GameplayObjectsHolder _gameplayObjects;
        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            _gameplayObjects.AllowedScatterCircleSpriteRenderer.color = _colorsSettings.FailedHitCircleColor;

            context.IsHitFailed.Value = true;
            var timerToShowFailedHit = _gameDefs.GameplaySettings.TimeToShowFailedHit;
            while (timerToShowFailedHit > 0 && context.IsDisposed == false)
            {
                timerToShowFailedHit -= Time.deltaTime;
                await Task.Yield();
            }
            context.IsHitFailed.Value = false;
        }
    }
}