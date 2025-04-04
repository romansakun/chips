using System.Threading.Tasks;
using Definitions;
using Gameplay;
using Gameplay.Chips;
using Installers;
using LogicUtility;
using UnityEngine;
using Zenject;

namespace UI.SelectingFromAllowedChips
{
    public abstract class BaseSelectingFromAllowedChipsViewModelQualifier : IQualifier<SelectingFromAllowedChipsViewModelContext> 
    {
        [Inject] protected CameraController _cameraController;
        [Inject] protected LayersSettings _layersSettings;


        public INode<SelectingFromAllowedChipsViewModelContext> Next { get; set; }
        public string GetLog() => GetType().Name;

        public virtual Task<float> ScoreAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            var score = Score(context);
            return Task.FromResult(score);
        }

        protected virtual float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            return 0;
        }

        protected bool IsEndOfDrag(SelectingFromAllowedChipsViewModelContext context, out Chip chip)
        {
            chip = null;
            if (context.Input.Item1 != DragInputType.OnEndDrag)
            {
                return false;
            }

            var position = context.StartSwipePosition;
            var ray = _cameraController.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out var hit, 100f, _layersSettings.ChipsLayerMask) == false)
            {
                return false;
            }

            var hitObject = hit.collider.gameObject;
            chip = hitObject.GetComponent<Chip>();
            if (chip == null)
            {
                return false;
            }

            return true;
        }
    }
}