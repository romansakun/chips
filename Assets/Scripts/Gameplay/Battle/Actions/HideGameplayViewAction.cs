using UnityEngine;

namespace Gameplay.Battle
{
    public class HideGameplayViewAction : BaseBattleLogicAction
    {
        // [Inject] private ChipsStack _chipsStack;
        // [Inject] private CameraController _cameraController;

        protected override void Execute(BattleContext context)
        {
            Debug.Log(nameof(HideGameplayViewAction));
        }
    }
}