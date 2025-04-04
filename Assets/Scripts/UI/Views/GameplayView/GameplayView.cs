using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class GameplayView : View, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Button _bitButton;
        [SerializeField] private Button _prepareForceButton;
        [SerializeField] private Button _prepareTorqueButton;
        [SerializeField] private Button _prepareHeightButton;
        [SerializeField] private Button _prepareAngleButton;
        [SerializeField] private GameObject _preparingButtonsContainer;
        [SerializeField] private RectTransform _preparingHitViewPartContainer;
        [SerializeField] private RectTransform _hitTimerContainer;
        [SerializeField] private RectTransform _leftNpcContainer;
        [SerializeField] private RectTransform _rightNpcContainer;

        //todo: remove it after tests
        [SerializeField] private Button _reloadButton;

        private TimerViewPart _hitTimer;
        private NpcViewPart _leftNpc;
        private NpcViewPart _rightNpc;
        private PreparingHitViewPart _preparingHit;
        private GameplayViewModel _viewModel;

        private void Awake()
        {
            _bitButton.onClick.AddListener(()=>_viewModel.OnBitButtonClick());
            _prepareForceButton.onClick.AddListener(()=>_viewModel.OnPrepareForceButtonClick());
            _prepareTorqueButton.onClick.AddListener(()=>_viewModel.OnPrepareTorqueButtonClick());
            _prepareHeightButton.onClick.AddListener(()=>_viewModel.OnPrepareHeightButtonClick());
            _prepareAngleButton.onClick.AddListener(()=>_viewModel.OnPrepareAngleButtonClick());

            _reloadButton.onClick.AddListener(()=>_viewModel.OnReloadButtonClick());
        }

        public override async UniTask Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);

            _leftNpc = await _guiManager.CreateViewPart<NpcViewPart>(_leftNpcContainer, _leftNpcContainer.name);
            _rightNpc = await _guiManager.CreateViewPart<NpcViewPart>(_rightNpcContainer, _rightNpcContainer.name);
            _hitTimer = await _guiManager.CreateViewPart<TimerViewPart>(_hitTimerContainer);
            _preparingHit = await _guiManager.CreateViewPart<PreparingHitViewPart>(_preparingHitViewPartContainer);

            _leftNpc.Subscribes(_viewModel.LeftNpc);
            _rightNpc.Subscribes(_viewModel.RightNpc);
            _hitTimer.Subscribes(_viewModel.HitTimer);
            _preparingHit.Subscribes(_viewModel.PreparingForceHit);
            _preparingHit.Subscribes(_viewModel.PreparingTorqueHit);
            _preparingHit.Subscribes(_viewModel.PreparingAngleHit);
            _preparingHit.Subscribes(_viewModel.PreparingHeightHit);
            _viewModel.ShowPreparingViewPart.Subscribe(IsPreparingViewPartVisibleChanged);
            _viewModel.ShowPreparingButtons.Subscribe(OnShowPreparingButtonsChanged);
            _viewModel.IsHitStarted.Subscribe(OnHitStarted);
            _viewModel.IsPlayerCanHitNow.Subscribe(OnPlayerCanHitNow);
            _viewModel.ProcessFirstPlayerMove();
        }

        private void ViewModelDispose()
        {
            _leftNpc.Unsubscribes(_viewModel.LeftNpc);
            _rightNpc.Unsubscribes(_viewModel.RightNpc);
            _hitTimer.Unsubscribes(_viewModel.HitTimer);
            _preparingHit.Unsubscribes(_viewModel.PreparingForceHit);
            _preparingHit.Unsubscribes(_viewModel.PreparingTorqueHit);
            _preparingHit.Unsubscribes(_viewModel.PreparingAngleHit);
            _preparingHit.Unsubscribes(_viewModel.PreparingHeightHit);
            _viewModel.ShowPreparingViewPart.Subscribe(IsPreparingViewPartVisibleChanged);
            _viewModel.IsHitStarted.Unsubscribe(OnHitStarted);
            _viewModel.IsPlayerCanHitNow.Unsubscribe(OnPlayerCanHitNow);
            _viewModel.ShowPreparingButtons.Unsubscribe(OnShowPreparingButtonsChanged);
            _viewModel.Dispose();
        }

        private void IsPreparingViewPartVisibleChanged(bool state)
        {
            _preparingHitViewPartContainer.gameObject.SetActive(state);
        }

        private void OnShowPreparingButtonsChanged(bool state)
        {
            _preparingButtonsContainer.gameObject.SetActive(state);
        }

        private void OnPlayerCanHitNow(bool state)
        {
            if (state == false)
                return;

            _bitButton.gameObject.SetActive(true);
            _preparingButtonsContainer.SetActive(true);
        }

        private void OnHitStarted(bool state)
        {
            if (state == false)
                return;

            _bitButton.gameObject.SetActive(false);
            _preparingButtonsContainer.SetActive(false);
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
            _bitButton.onClick.RemoveAllListeners();
            _reloadButton.onClick.RemoveAllListeners();
            _prepareForceButton.onClick.RemoveAllListeners();
            _prepareTorqueButton.onClick.RemoveAllListeners();
            _prepareHeightButton.onClick.RemoveAllListeners();
            _prepareAngleButton.onClick.RemoveAllListeners();
            if (_viewModel != null)
            {
                ViewModelDispose();
            }
        }

    }
}