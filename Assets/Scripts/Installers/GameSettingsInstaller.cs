using System;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller 
    {
        [SerializeField] private GameRules _gameRules;
        [SerializeField] private ChipsSettings _chipsSettings;
        [SerializeField] private SoundsSettings _soundsSettings;

        public override void InstallBindings()
        {
            Container.Bind<GameRules>().FromInstance(_gameRules).AsSingle();
            Container.Bind<ChipsSettings>().FromInstance(_chipsSettings).AsSingle();
            Container.Bind<SoundsSettings>().FromInstance(_soundsSettings).AsSingle();
            Container.Bind<LayersSettings>().AsSingle();
        }
    }

    [Serializable]
    public class GameRules
    {
        public float AllowedScatterRadius;
        public float AllowedSlopeAngle;
        public float MaxTimeToWaitForRepeatHit;
    }

    [Serializable]
    public class ChipsSettings
    {
        public Mesh[] ChipsMeshes;
        public Material ChipsMaterial;
    }

    [Serializable]
    public class SoundsSettings
    {
        public AudioClip GroundChipsHitSound;
        public AudioClip BackroundMusic;
    }

    public class LayersSettings
    {
        public int GroundLayer { get; } = LayerMask.NameToLayer("Ground");
        public int WallLayer { get; } = LayerMask.NameToLayer("Wall");
    }
}