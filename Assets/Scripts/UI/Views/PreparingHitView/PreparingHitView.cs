using Cysharp.Threading.Tasks;
using Installers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PreparingHit
{
    public class PreparingHitView : View
    {
        [Inject] private ColorsSettings _colorsSettings;

        [SerializeField] private Image _infoImage;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _forceButton;
        [SerializeField] private Button _angleButton;
        [SerializeField] private Button _heightButton;
        [SerializeField] private Button _torqueButton;
        [SerializeField] private Scrollbar _valueScrollbar;
        [SerializeField] private TextMeshProUGUI _valueText;

        private PreparingHitViewModel _viewModel;

        private void Awake()
        {
            _valueScrollbar.onValueChanged.AddListener((v) => _viewModel.ProcessSliderValue(v));
            _forceButton.onClick.AddListener(() => _viewModel.OnForceButtonClicked());
            _angleButton.onClick.AddListener(() => _viewModel.OnAngleButtonClicked());
            _heightButton.onClick.AddListener(() => _viewModel.OnHeightButtonClicked());
            _torqueButton.onClick.AddListener(() => _viewModel.OnTorqueButtonClicked());
            _closeButton.onClick.AddListener(Close);
        }

        public override UniTask Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
            _viewModel.InfoSprite.Subscribe(OnInfoSpriteChanged);
            _viewModel.ValueScrollbar.Subscribe(OnValueSliderChanged);
            _viewModel.ValueText.Subscribe(OnValueTextChanged);
            return UniTask.CompletedTask;
        }

        private void DisposeViewModel()
        {
            _viewModel.InfoSprite.Unsubscribe(OnInfoSpriteChanged);
            _viewModel.ValueScrollbar.Unsubscribe(OnValueSliderChanged);
            _viewModel.ValueText.Unsubscribe(OnValueTextChanged);
            _viewModel.Dispose();
        }

        private void OnValueSliderChanged(float value)
        {
            _valueScrollbar.value = value;
        }

        private void OnInfoSpriteChanged(Sprite infoSprite)
        {
            _infoImage.sprite = infoSprite;
        }

        private void OnValueTextChanged(string value)
        {
            _valueText.text = value;
        }

        private void UnselectAllButtons()
        {
            _forceButton.GetComponent<Image>().color = _colorsSettings.NotSelectedPrepareHitButtonColor;
            _angleButton.GetComponent<Image>().color = _colorsSettings.NotSelectedPrepareHitButtonColor;
            _heightButton.GetComponent<Image>().color = _colorsSettings.NotSelectedPrepareHitButtonColor;
            _torqueButton.GetComponent<Image>().color = _colorsSettings.NotSelectedPrepareHitButtonColor;
        }

        public void SelectForceButton()
        {
            UnselectAllButtons();
            _forceButton.GetComponent<Image>().color = _colorsSettings.SelectedPrepareHitButtonColor;
        }

        public void SelectAngleButton()
        {
            UnselectAllButtons();
            _angleButton.GetComponent<Image>().color = _colorsSettings.SelectedPrepareHitButtonColor;
        }

        public void SelectHeightButton()
        {
            UnselectAllButtons();
            _heightButton.GetComponent<Image>().color = _colorsSettings.SelectedPrepareHitButtonColor;
        }

        public void SelectTorqueButton()
        {
            UnselectAllButtons();
            _torqueButton.GetComponent<Image>().color = _colorsSettings.SelectedPrepareHitButtonColor;
        }

        private void OnDestroy()
        {
            _forceButton.onClick.RemoveAllListeners();
            _angleButton.onClick.RemoveAllListeners();
            _heightButton.onClick.RemoveAllListeners();
            _torqueButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            _valueScrollbar.onValueChanged.RemoveAllListeners();
            
            DisposeViewModel();
        }

    }
}