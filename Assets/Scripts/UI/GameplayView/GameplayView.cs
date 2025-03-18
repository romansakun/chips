using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameplayView : View, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Button _prepareButton;
        [SerializeField] private Button _bitButton;
        [SerializeField] private Button _reloadButton;

        private GameplayViewModel _viewModel;

        private void Awake()
        {
            _prepareButton.onClick.AddListener(()=>_viewModel.OnPrepareButtonClick());
            _bitButton.onClick.AddListener(()=>_viewModel.OnBitButtonClick());
            _reloadButton.onClick.AddListener(()=>_viewModel.OnReloadButtonClick());
        }

        public override void Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
            _viewModel.ShowHitTimer.Subscribe(ShowHitTimerChanged);
            _viewModel.HitTimer.Subscribe(HitTimerChanged);
        }

        private void ShowHitTimerChanged(bool state)
        {
            Debug.Log($"Show hit timer = {state}");
        }

        private void HitTimerChanged(float value)
        {
            Debug.Log($"hit timer = {value}");
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
            _reloadButton.onClick.RemoveAllListeners();
            _viewModel.Dispose();
        }
    }
}