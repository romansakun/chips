using Definitions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RockPaperScissorsView : View
    {
        [SerializeField] private TextMeshProUGUI _titleInfoText;
        [SerializeField] private TimerViewBit _timer;
        [SerializeField] private NpcViewBit _leftNpc;
        [SerializeField] private NpcViewBit _rightNpc;
        [SerializeField] private TextMeshProUGUI _playerInfoText;
        [SerializeField] private GameObject _handsButtonsHolder;
        [SerializeField] private Button _skissorsButton;
        [SerializeField] private Button _rockButton;
        [SerializeField] private Button _paperButton;  
        [SerializeField] private Image _skissorsButtonImage;
        [SerializeField] private Image _rockButtonImage;
        [SerializeField] private Image _paperButtonImage;
        [SerializeField] private Image _playerHandImage;
        [SerializeField] private RectTransform _playerHandImageRectTransform;

        private RockPaperScissorsViewModel _viewModel;

        private void Awake()
        {
            _skissorsButton.onClick.AddListener(() => _viewModel.OnPlayerHandClicked(RockPaperScissorsHand.Scissors));
            _rockButton.onClick.AddListener(() => _viewModel.OnPlayerHandClicked(RockPaperScissorsHand.Rock));
            _paperButton.onClick.AddListener(() => _viewModel.OnPlayerHandClicked(RockPaperScissorsHand.Paper));
        }

        public override void Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
            _leftNpc.Subscribes(_viewModel.LeftNpcViewBitModel);
            _rightNpc.Subscribes(_viewModel.RightNpcViewBitModel);
            _timer.Subscribes(_viewModel.TimerViewBitModel);
            _viewModel.PlayerHandSprite.Subscribe(OnPlayerHandSpriteChanged);
            _viewModel.PlayerChosenHandScale.Subscribe(OnPlayerChosenHandScaleChanged);
            _viewModel.PlayerInfoText.Subscribe(OnPlayerInfoTextChanged);
            _viewModel.ShowPlayerHand.Subscribe(OnPlayerHandVisibleChanged);
            _viewModel.ShowHandsButtons.Subscribe(OnPlayerHandsButtonsVisibleChanged);
            _viewModel.RockButtonSprite.Subscribe(OnRockButtonSpriteChanged);
            _viewModel.PaperButtonSprite.Subscribe(OnPaperButtonSpriteChanged);
            _viewModel.ScissorsButtonSprite.Subscribe(OnScissorsButtonSpriteChanged);
            _viewModel.TitleInfoText.Subscribe(OnTitleTextChanged);
            _viewModel.ShowTitleInfoText.Subscribe(OnTitleTextVisibleChanged);
            _viewModel.ShowPlayerInfoText.Subscribe(OnPlayerInfoTextVisibleChanged);
        }

        private void ViewModelDispose()
        {
            _leftNpc.Unsubscribes(_viewModel.LeftNpcViewBitModel);
            _rightNpc.Unsubscribes(_viewModel.RightNpcViewBitModel);
            _timer.Unsubscribes(_viewModel.TimerViewBitModel);
            _viewModel.PlayerHandSprite.Unsubscribe(OnPlayerHandSpriteChanged);
            _viewModel.PlayerChosenHandScale.Unsubscribe(OnPlayerChosenHandScaleChanged);
            _viewModel.PlayerInfoText.Unsubscribe(OnPlayerInfoTextChanged);
            _viewModel.ShowPlayerHand.Unsubscribe(OnPlayerHandVisibleChanged);
            _viewModel.ShowHandsButtons.Unsubscribe(OnPlayerHandsButtonsVisibleChanged);
            _viewModel.RockButtonSprite.Unsubscribe(OnRockButtonSpriteChanged);
            _viewModel.PaperButtonSprite.Unsubscribe(OnPaperButtonSpriteChanged);
            _viewModel.ScissorsButtonSprite.Unsubscribe(OnScissorsButtonSpriteChanged);
            _viewModel.TitleInfoText.Unsubscribe(OnTitleTextChanged);
            _viewModel.ShowTitleInfoText.Unsubscribe(OnTitleTextVisibleChanged);
            _viewModel.ShowPlayerInfoText.Unsubscribe(OnPlayerInfoTextVisibleChanged);
            _viewModel.Dispose();
        }

        private void OnTitleTextChanged(string text)
        { 
            _titleInfoText.text = text;
        }

        private void OnTitleTextVisibleChanged(bool state)
        {
            _titleInfoText.gameObject.SetActive(state);
        }

        private void OnPlayerInfoTextVisibleChanged(bool state)
        {
            _playerInfoText.gameObject.SetActive(state);
        }

        private void OnPlayerChosenHandScaleChanged(Vector3 scale)
        {
            _playerHandImageRectTransform.localScale = scale;
        }

        private void OnRockButtonSpriteChanged(Sprite sprite)
        {
            _rockButtonImage.sprite = sprite;
        }

        private void OnPaperButtonSpriteChanged(Sprite sprite)
        {
            _paperButtonImage.sprite = sprite;
        }

        private void OnScissorsButtonSpriteChanged(Sprite sprite)
        {
            _skissorsButtonImage.sprite = sprite;
        }

        private void OnPlayerInfoTextChanged(string info)
        {
            _playerInfoText.text = info;
        }

        private void OnPlayerHandSpriteChanged(Sprite sprite)
        {
            _playerHandImage.sprite = sprite;
        }

        private void OnPlayerHandVisibleChanged(bool state)
        {
            _playerHandImage.gameObject.SetActive(state);
        }

        private void OnPlayerHandsButtonsVisibleChanged(bool state)
        {
            _handsButtonsHolder.SetActive(state);
        }

        private void OnDestroy()
        {
            _skissorsButton.onClick.RemoveAllListeners();
            _rockButton.onClick.RemoveAllListeners();
            _paperButton.onClick.RemoveAllListeners();
            if (_viewModel != null)
            {
                ViewModelDispose();
            }
        }
    }
}