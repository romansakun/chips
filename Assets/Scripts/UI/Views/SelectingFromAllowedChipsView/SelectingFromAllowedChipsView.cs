using Definitions;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class SelectingFromAllowedChipsView : View, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private LocalizationText _needToSelectChipsCountText;
        [SerializeField] private LocalizationText _betChipsCountText;
        [SerializeField] private RepeatButton _selectCurrentChipButton;
        [SerializeField] private RepeatButton _skipCurrentChipButton;
        [SerializeField] private RepeatButton _skipBetChipButton;
        [SerializeField] private RepeatButton _moveSkippedToWatchingChipButton;
        [SerializeField] private RectTransform _selectCurrentChipRectTransform;
        [SerializeField] private RectTransform _skipCurrentChipRectTransform;
        [SerializeField] private RectTransform _skipBetChipRectTransform;
        [SerializeField] private RectTransform _moveSkippedToWatchingChipRectTransform;

        [Inject] private GameDefs _gameDefs;

        private SelectingFromAllowedChipsViewModel _viewModel;


        private void Awake()
        {
            _selectCurrentChipButton.OnClick += OnSelectCurrentChipButtonClicked;
            _skipCurrentChipButton.OnClick += OnSkipCurrentChipButtonClicked;
            _skipBetChipButton.OnClick += OnSkipBetChipButtonClicked;
            _moveSkippedToWatchingChipButton.OnClick += OnMoveSkippedToWatchingChipButtonClicked;
        }

        private void Start()
        {
            _betChipsCountText.UpdateText("SELECTED_CHIPS_COUNT", 0);
            _needToSelectChipsCountText.UpdateText("NEED_TO_SELECT_CHIPS_COUNT", 0);
        }

        public override void Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
            _viewModel.ShowMoveSkippedToWatchingChipButton.Subscribe(ShowMoveSkippedToWatchingChipButtonChanged);
            _viewModel.CurrentWatchingChipCanvasPosition.Subscribe(OnCurrentWatchingChipCanvasPositionChanged);
            _viewModel.ShowSelectWatchingChipToBetButton.Subscribe(ShowSelectWatchingChipToBetButtonChanged);
            _viewModel.ShowSkipCurrentChipButton.Subscribe(ShowSkipCurrentChipButtonChanged);
            _viewModel.ShowSkipBetChipButton.Subscribe(ShowSkipBetChipButtonChanged);
            _viewModel.BetChipsCount.Subscribe(OnBetChipsCountChanged);
        }

        private void ViewModelDispose()
        {
            _viewModel.ShowMoveSkippedToWatchingChipButton.Unsubscribe(ShowMoveSkippedToWatchingChipButtonChanged);
            _viewModel.CurrentWatchingChipCanvasPosition.Unsubscribe(OnCurrentWatchingChipCanvasPositionChanged);
            _viewModel.ShowSelectWatchingChipToBetButton.Unsubscribe(ShowSelectWatchingChipToBetButtonChanged);
            _viewModel.ShowSkipCurrentChipButton.Unsubscribe(ShowSkipCurrentChipButtonChanged);
            _viewModel.ShowSkipBetChipButton.Unsubscribe(ShowSkipBetChipButtonChanged);
            _viewModel.BetChipsCount.Unsubscribe(OnBetChipsCountChanged);
            _viewModel.Dispose();
        }

        private void OnCurrentWatchingChipCanvasPositionChanged(Vector2 position)
        {
            _selectCurrentChipRectTransform.anchoredPosition = position + _gameDefs.SelectingChipsForBetSettings.SelectCurrentChipButtonOffset;
            _skipCurrentChipRectTransform.anchoredPosition = position + _gameDefs.SelectingChipsForBetSettings.SkipCurrentChipButtonOffset;
            _skipBetChipRectTransform.anchoredPosition = position + _gameDefs.SelectingChipsForBetSettings.SkipBetChipButtonOffset;
            _moveSkippedToWatchingChipRectTransform.anchoredPosition = position + _gameDefs.SelectingChipsForBetSettings.MoveSkippedToWatchingChipButtonOffset;
        }

        private void ShowSkipCurrentChipButtonChanged(bool state)
        {
            _skipCurrentChipButton.gameObject.SetActive(state);
        }

        private void OnBetChipsCountChanged(int count = 0)
        {
            _betChipsCountText.UpdateText(count);
        }

        private void ShowSkipBetChipButtonChanged(bool state)
        {
            _skipBetChipButton.gameObject.SetActive(state);
        }

        private void ShowMoveSkippedToWatchingChipButtonChanged(bool state)
        {
            _moveSkippedToWatchingChipButton.gameObject.SetActive(state);
        }

        private void ShowSelectWatchingChipToBetButtonChanged(bool state)
        {
            _selectCurrentChipButton.gameObject.SetActive(state);
        }

        private void OnSelectCurrentChipButtonClicked()
        {
            _viewModel.SelectCurrentChipButtonClicked();
        }

        private void OnSkipCurrentChipButtonClicked()
        {
            _viewModel.SkipCurrentChipButtonClicked();
        }

        private void OnMoveSkippedToWatchingChipButtonClicked()
        {
            _viewModel.MoveSkippedToWatchingChipButtonClicked();
        }

        private void OnSkipBetChipButtonClicked()
        {
            _viewModel.SkipBetChipButtonClicked();
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

        private void OnDestroy()
        {
            _moveSkippedToWatchingChipButton.OnClick -= OnMoveSkippedToWatchingChipButtonClicked;
            _selectCurrentChipButton.OnClick -= OnSelectCurrentChipButtonClicked;
            _skipCurrentChipButton.OnClick -= OnSkipCurrentChipButtonClicked;
            _skipBetChipButton.OnClick -= OnSkipBetChipButtonClicked;
            if (_viewModel != null)
            {
                ViewModelDispose();
            }
        }

    }
}