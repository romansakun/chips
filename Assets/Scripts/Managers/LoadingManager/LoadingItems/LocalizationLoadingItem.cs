using Cysharp.Threading.Tasks;
using Definitions;
using Model;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LocalizationLoadingItem : ILoadingItem
    {
        [Inject] private UserContextRepository _userContext;
        [Inject] private LocalizationManager _localizationManager;
        [Inject] private GameDefs _gameDefs;

        public async UniTask Load()
        {
            var language = _userContext.GetLocalizationLanguage();
            if (string.IsNullOrEmpty(language))
                language = Application.systemLanguage.ToString();

            if (_localizationManager.TrySetLocalization(language) == false)
                _localizationManager.TrySetLocalization(SystemLanguage.English);

            await UniTask.Yield();
        }
    }
}