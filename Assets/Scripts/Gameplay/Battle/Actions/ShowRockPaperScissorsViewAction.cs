using UnityEngine;

namespace Gameplay.Battle
{
    public class ShowRockPaperScissorsViewAction : BaseBattleLogicAction
    {
        // [Inject] private ChipsStack _chipsStack;
        // [Inject] private CameraController _cameraController;

        protected override void Execute(BattleContext context)
        {
            Debug.Log(nameof(ShowRockPaperScissorsViewAction));
        }
    }
}