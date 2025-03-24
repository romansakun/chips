using Gameplay;
using Gameplay.Chips;
using Installers;
using UnityEngine;
using Zenject;
using Definitions;

namespace UI
{
    public class IsFirstSelectedChipClickedQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        [Inject] private CameraController _cameraController;
        [Inject] private LayersSettings _layersSettings;

        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.Input.Item1 != InputType.OnPointerClick)
                return 0;

            var position = context.Input.Item2.position;
            var ray = _cameraController.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out var hit, 100f, _layersSettings.ChipsLayerMask) == false) 
                return 0;

            var hitObject = hit.collider.gameObject;
            var chip = hitObject.GetComponent<Chip>();
            if (chip == null)
                return 0;

            if (context.SelectedChips.IndexOf(chip) != 0)
                return 0;

            context.Input = (InputType.None, default);
            return 1;
        }
    }
}