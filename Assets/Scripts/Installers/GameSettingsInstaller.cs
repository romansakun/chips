using System;
using Definitions;
using Gameplay.Chips;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller 
    {
        [SerializeField] private GameDefsTextAssetProvider _gameDefsTextAssetProvider;
        [SerializeField] private PrefabsSettings _prefabsSettings;
        [SerializeField] private SoundsSettings _soundsSettings;
        [SerializeField] private ColorsSettings _colorsSettings;

#if WITH_CHEATS
        [SerializeField] private GameObject _ingameDebugConsolePrefab;
#endif

        public override void InstallBindings()
        {
            Container.Bind<PrefabsSettings>().FromInstance(_prefabsSettings).AsSingle();
            Container.Bind<SoundsSettings>().FromInstance(_soundsSettings).AsSingle();
            Container.Bind<ColorsSettings>().FromInstance(_colorsSettings).AsSingle();
            Container.Bind<GameDefs>().AsSingle();
            Container.Bind<LayersSettings>().AsSingle();
#if WITH_CHEATS
            Instantiate(_ingameDebugConsolePrefab);
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _gameDefsTextAssetProvider.DefinitionsData = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Content/Definitions.json");
        }
#endif

    }

    [Serializable]
    public class GameDefsTextAssetProvider
    {
        public TextAsset DefinitionsData;
    }

    [Serializable]
    public class PrefabsSettings
    {
        public Chip ChipPrefab;
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
        public int ChipsLayer { get; } = LayerMask.NameToLayer("Chips");
        public int ChipsLayerMask { get; } = LayerMask.GetMask("Chips");
    }
}