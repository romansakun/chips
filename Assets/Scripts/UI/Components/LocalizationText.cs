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

        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _localizationManager.OnLocalizationChanged += UpdateText;
            UpdateText();
        }

        private void UpdateText()
        {
            if (string.IsNullOrEmpty(LocalizationKey))
                return;

            _textMeshPro.text = _localizationManager.GetText(LocalizationKey);
        }

        private void OnDestroy()
        {
            _localizationManager.OnLocalizationChanged -= UpdateText;
        }
    }
}