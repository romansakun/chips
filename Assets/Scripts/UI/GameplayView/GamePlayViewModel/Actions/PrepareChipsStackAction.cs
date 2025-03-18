using Gameplay;
using Gameplay.Chips;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PrepareChipsStackAction : BaseGameplayViewModelAction
    {
        [Inject] private Chip.Factory _chipFactory;
        [Inject] private ChipsStack _chipsStack;
        [Inject] private CameraController _cameraController;

        protected override void Execute(GameplayViewModelContext context)
        {
            
            _chipsStack.RemoveAllChips();
            for (int i = 0; i < 12; i++)
            {
                var chip = _chipFactory.Create();
                chip.Rigidbody.isKinematic = true;
                chip.Transform.rotation = Quaternion.identity;
                //chip.Rigidbody.automaticCenterOfMass = false;
                //chip.Rigidbody.centerOfMass = new Vector3(Random.Range(-0.15f, 0.15f), 0, Random.Range(-0.15f, 0.15f));
                chip.Rigidbody.automaticCenterOfMass = true;
                chip.Transform.position = new Vector3(0, 6 + i * .25f, 0);
                //chip.Collider.sharedMaterial = null
                _chipsStack.AddChip(chip);
            }
            _cameraController.ResetPosition();
        }
    }
}