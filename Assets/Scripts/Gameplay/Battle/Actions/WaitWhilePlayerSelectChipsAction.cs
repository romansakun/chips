using Gameplay;
using UnityEngine;
using Zenject;

namespace Gameplay.Battle
{
    public class WaitWhilePlayerSelectChipsAction : BaseBattleLogicAction
    {
        // [Inject] private ChipsStack _chipsStack;
        // [Inject] private CameraController _cameraController;

        protected override void Execute(BattleContext context)
        {
            Debug.Log(nameof(WaitWhilePlayerSelectChipsAction));
        }
    }
}