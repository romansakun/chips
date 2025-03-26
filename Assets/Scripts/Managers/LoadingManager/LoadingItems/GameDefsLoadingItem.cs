using Cysharp.Threading.Tasks;
using Definitions;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameDefsLoadingItem : ILoadingItem
    {
        [Inject] private AddressableManager _addressableManager;
        [Inject] private GameDefs _gameDefs;

        public async UniTask Load()
        {
            var gameDefsText = await _addressableManager.LoadAsync<TextAsset>("Definitions");

            var settings = new JsonSerializerSettings
            {
                Converters = new JsonConverter[]
                {
                    new Vector3Converter(),
                    new Vector2Converter()
                },
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                NullValueHandling = NullValueHandling.Include
            };
            JsonConvert.PopulateObject(gameDefsText.text, _gameDefs, settings);

            await UniTask.Yield();
        }
    }
}