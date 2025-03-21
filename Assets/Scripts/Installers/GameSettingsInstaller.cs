using System;
using Definitions;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller 
    {
        [SerializeField] private GameDefsTextAssetProvider gameDefsTextAssetProvider;
        [SerializeField] private SoundsSettings _soundsSettings;
        [SerializeField] private ColorsSettings _colorsSettings;
#if WITH_CHEATS
        [SerializeField] private GameObject IngameDebugConsolePrefab;
#endif
        public override void InstallBindings()
        {
            var gameDefs = JsonConvert.DeserializeObject<GameDefs>(gameDefsTextAssetProvider.DefinitionsData.text);
            Container.Bind<GameDefs>().FromInstance(gameDefs).AsSingle();
            Container.Bind<SoundsSettings>().FromInstance(_soundsSettings).AsSingle();
            Container.Bind<ColorsSettings>().FromInstance(_colorsSettings).AsSingle();
            Container.Bind<LayersSettings>().AsSingle();

#if WITH_CHEATS
            Instantiate(IngameDebugConsolePrefab);
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameDefsTextAssetProvider.DefinitionsData = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Content/Definitions.json");
        }
#endif

    }

    [Serializable]
    public class GameDefsTextAssetProvider
    {
        public TextAsset DefinitionsData;
    }

    [Serializable]
    public class SoundsSettings
    {
        public AudioClip GroundChipsHitSound;
        public AudioClip BackroundMusic;
    }

    [Serializable]
    public class ColorsSettings
    {
        public Color DefaultCircleColor;
        public Color FailedHitCircleColor;
        public Color SuccessHitCircleColor;
    }

    public class LayersSettings
    {
        public int GroundLayer { get; } = LayerMask.NameToLayer("Ground");
        public int WallLayer { get; } = LayerMask.NameToLayer("Wall");
    }
}