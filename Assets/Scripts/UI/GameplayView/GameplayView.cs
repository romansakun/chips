using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameplayView : View, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject _successMessage;
        [SerializeField] private GameObject _failMessage;
        [SerializeField] private GameObject _hitTimer;
        [SerializeField] private TextMeshProUGUI _hitTimerText;
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
            _viewModel.IsHitFailed.Subscribe(IsHitFailedChanged);
            _viewModel.IsHitSuccess.Subscribe(IsHitSuccessChanged);
        }

        private void ViewModelDispose()
        {
            _viewModel.ShowHitTimer.Unsubscribe(ShowHitTimerChanged);
            _viewModel.HitTimer.Unsubscribe(HitTimerChanged);
            _viewModel.IsHitFailed.Unsubscribe(IsHitFailedChanged);
            _viewModel.IsHitSuccess.Unsubscribe(IsHitSuccessChanged);
            _viewModel.Dispose();
        }

        private void IsHitSuccessChanged(bool state)
        {
            _successMessage.SetActive(state);
            if (state == false)
                _bitButton.gameObject.SetActive(true);
        }

        private void IsHitFailedChanged(bool state)
        {
            _failMessage.SetActive(state);
            if (state == false)
                _bitButton.gameObject.SetActive(true);
        }

        private void ShowHitTimerChanged(bool state)
        {
            _hitTimer.SetActive(state);
            if (state)
                _bitButton.gameObject.SetActive(false);
        }

        private void HitTimerChanged(float value)
        {
            _hitTimerText.SetText(Mathf.CeilToInt(value).ToString());
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
            if (_viewModel != null)
            {
                ViewModelDispose();
            }
        }

    }
}