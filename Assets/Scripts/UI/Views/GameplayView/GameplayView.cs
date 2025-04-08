using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class GameplayView : View
    {
        [SerializeField] private Button _bitButton;
        [SerializeField] private Button _preparingButton;
        [SerializeField] private RectTransform _hitTimerContainer;
        [SerializeField] private RectTransform _leftNpcContainer;
        [SerializeField] private RectTransform _rightNpcContainer;

        //todo: remove it after tests
        [SerializeField] private Button _reloadButton;

        private TimerViewPart _hitTimer;
        private NpcViewPart _leftNpc;
        private NpcViewPart _rightNpc;

        private GameplayViewModel _viewModel;

        private void Awake()
        {
            _bitButton.onClick.AddListener(()=>_viewModel.OnBitButtonClick());
            _preparingButton.onClick.AddListener(()=>_viewModel.OnPrepareButtonClick());

            _reloadButton.onClick.AddListener(()=>_viewModel.OnReloadButtonClick());
        }

        public override async UniTask Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);

            _leftNpc = await _guiManager.CreateViewPart<NpcViewPart>(_leftNpcContainer, _leftNpcContainer.name);
            _rightNpc = await _guiManager.CreateViewPart<NpcViewPart>(_rightNpcContainer, _rightNpcContainer.name);
            _hitTimer = await _guiManager.CreateViewPart<TimerViewPart>(_hitTimerContainer);

            _leftNpc.Subscribes(_viewModel.LeftNpc);
            _rightNpc.Subscribes(_viewModel.RightNpc);
            _hitTimer.Subscribes(_viewModel.HitTimer);

            _viewModel.IsPlayerCanHitNow.Subscribe(OnPlayerCanHitNow);
            _viewModel.ProcessFirstPlayerMove();
        }

        private void ViewModelDispose()
        {
            _leftNpc.Unsubscribes(_viewModel.LeftNpc);
            _rightNpc.Unsubscribes(_viewModel.RightNpc);
            _hitTimer.Unsubscribes(_viewModel.HitTimer);

            _viewModel.IsPlayerCanHitNow.Unsubscribe(OnPlayerCanHitNow);
            _viewModel.Dispose();
        }

        private void OnPlayerCanHitNow(bool state)
        {
            _bitButton.gameObject.SetActive(state);
            _preparingButton.gameObject.SetActive(state);
        }

        private void OnDestroy()
        {
            _bitButton.onClick.RemoveAllListeners();
            _preparingButton.onClick.RemoveAllListeners();
            _reloadButton.onClick.RemoveAllListeners();
            if (_viewModel != null)
            {
                ViewModelDispose();
            }
        }

    }
}