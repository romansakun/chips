using Managers;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowAllowedPlayerChipsViewAction : BaseBattleLogicAction
    {
        // [Inject] private AudioManager _audioManager;
        // [Inject] private ChipsStack _chipsStack;
        // [Inject] private CameraController _cameraController;

        protected override void Execute(BattleContext context)
        {
            Debug.Log(nameof(ShowAllowedPlayerChipsViewAction));
        }
    }
}