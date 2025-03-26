using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class View : MonoBehaviour
    {
        [Inject] protected GuiManager _guiManager;

        public short OverridedSortingOrder = -1;
        public RectTransform RectTransform => GetComponent<RectTransform>();

        public abstract void Initialize(ViewModel viewModel);

        protected void UpdateViewModel<T>(ref T oldViewModel, ViewModel newViewModel) where T : ViewModel
        {
            oldViewModel?.Dispose();
            oldViewModel = (T) newViewModel;
        }
        
        [ContextMenu("Close")]
        public void Close()
        {
            _guiManager.Close(this);
        }
    }

}