using System;
using Definitions;
using Gameplay.Chips;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller 
    {
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

            SignalsInstaller.Install(Container);

#if WITH_CHEATS
            Instantiate(_ingameDebugConsolePrefab);
#endif
        }
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
        public Color WhiteTextColor;
    }

    public class LayersSettings
    {
        public int GroundLayer { get; } = LayerMask.NameToLayer("Ground");
        public int ChipsLayer { get; } = LayerMask.NameToLayer("Chips");
        public int ChipsLayerMask { get; } = LayerMask.GetMask("Chips");
    }
}