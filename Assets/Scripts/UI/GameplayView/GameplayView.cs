using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameplayView : View, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Button _prepareButton;
        [SerializeField] private Button _bitButton;

        private GameplayViewModel _viewModel;

        private void Awake()
        {
            _prepareButton.onClick.AddListener(()=>_viewModel.OnPrepareButtonClick());
            _bitButton.onClick.AddListener(()=>_viewModel.OnBitButtonClick());
        }

        public override void Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _viewModel.OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _viewModel.OnEndDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _viewModel.OnDrag(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _viewModel.OnPointerClick(eventData);
        }

        private void OnDestroy()
        {
            _prepareButton.onClick.RemoveAllListeners();
            _bitButton.onClick.RemoveAllListeners();
            _viewModel.Dispose();
        }
    }
}