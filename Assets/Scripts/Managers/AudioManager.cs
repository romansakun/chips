using Installers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _hitAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;

        [Inject] private SoundsSettings _soundsSettings;


        private bool _isReadyForGroundChipsHitSound;

        public void PrepareForGroundChipsHitSound()
        {
            _isReadyForGroundChipsHitSound = true;
        }

        private void Start()
        {
            _musicAudioSource.clip = _soundsSettings.BackroundMusic;
            _musicAudioSource.volume = .15f;
            _musicAudioSource.loop = true;
            _musicAudioSource.Play();
        }

        public void PlayGroundChipsHitSound()
        {
            if (_isReadyForGroundChipsHitSound == false)
                return;
            
            _isReadyForGroundChipsHitSound = false;
            _hitAudioSource.spatialBlend = 1f;
            _hitAudioSource.pitch = Random.Range(.8f, 1.2f);
            _hitAudioSource.volume = Random.Range(.85f, 1f);
            _hitAudioSource.clip = _soundsSettings.GroundChipsHitSound;
            _hitAudioSource.Play();
        }
    }
}