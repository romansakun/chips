using UnityEngine;

namespace Gameplay.Battle
{
    public class WaitWhileOnePlayerWinAction : BaseBattleLogicAction
    {
        // [Inject] private ChipsStack _chipsStack;
        // [Inject] private CameraController _cameraController;

        protected override void Execute(BattleContext context)
        {
            Debug.Log(nameof(WaitWhileOnePlayerWinAction));
        }
    }
}