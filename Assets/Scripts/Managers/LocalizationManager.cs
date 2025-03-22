using System;
using Definitions;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LocalizationManager : IInitializable, IDisposable
    {
        [Inject] private GameDefs _gameDefs;

        public event Action OnLocalizationChanged;

        private LocalizationDef _currentLocalization;

        public void Initialize()
        {
            TrySetLocalization(Application.systemLanguage);
#if WITH_CHEATS
            AddDebugCommands();
#endif
        }

        public bool TrySetLocalization(SystemLanguage language)
        {
            if (_gameDefs.Localizations.TryGetValue(language.ToString(), out var localization))
            {
                _currentLocalization = localization;
                OnLocalizationChanged?.Invoke();
                return true;
            }
            else
            {
                Debug.LogWarning($"Localization for {language} not found");
                return false;
            }
        }

        public string GetText(string key)
        {
            if (_currentLocalization.LocalizationText.TryGetValue(key, out var text))
                return text;

            Debug.LogWarning($"{key} not found in {_currentLocalization.Id} localization");
            return key;
        }

        public void Dispose()
        {
            OnLocalizationChanged = null;
#if WITH_CHEATS
            RemoveDebugCommands();
#endif
        }

#if WITH_CHEATS
        private void AddDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.AddCommand<string>(nameof(Localization).ToLower(), string.Empty, Localization);
        }

        private void RemoveDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.RemoveCommand<string>(Localization);
        }

        private void Localization(string language)
        {
            TrySetLocalization(Enum.Parse<SystemLanguage>(language));
        }
#endif
        
    }
}