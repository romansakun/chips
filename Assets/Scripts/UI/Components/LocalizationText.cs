using Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationText : MonoBehaviour 
    {
        [Inject] private LocalizationManager _localizationManager;

        public string LocalizationKey;
        public object[] Args;

        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _localizationManager.OnLocalizationChanged += UpdateTextInternal;
            UpdateText(LocalizationKey);
        }

        public void UpdateText(string localizationKey)
        {
            if (string.IsNullOrEmpty(localizationKey))
                return;

            LocalizationKey = localizationKey;
            UpdateTextInternal();
        }

        public void UpdateText(string localizationKey, params object[] args)
        {
            if (string.IsNullOrEmpty(localizationKey))
                return;

            LocalizationKey = localizationKey;
            Args = args;
            UpdateTextInternal();
        }

        public void UpdateText(params object[] args)
        {
            if (string.IsNullOrEmpty(LocalizationKey))
                return;

            Args = args;
            UpdateTextInternal();
        }

        private void UpdateTextInternal()
        {
            if (Args == null || Args.Length == 0)
                _textMeshPro.text = _localizationManager.GetText(LocalizationKey, this);
            else
                _textMeshPro.text = string.Format(_localizationManager.GetText(LocalizationKey, this), Args);
        }

        private void OnDestroy()
        {
            _localizationManager.OnLocalizationChanged -= UpdateTextInternal;
        }
    }
}