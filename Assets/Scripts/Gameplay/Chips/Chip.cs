using System;
using Installers;
using Managers;
using UnityEngine;
using Zenject;

namespace Gameplay.Chips
{
    public class Chip : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        //private const float SQR_SPEED_THRESHOLD = 0.15f;
        private const float SQR_LAST_POSITION_THRESHOLD = 0.00025f;
        private const float LAST_ANGLE_THRESHOLD = 2f;
        private const int WATHCHING_REST_FRAMES = 60;

        [Inject] private AudioManager _audioManager;
        [Inject] private LayersSettings _layersSettings;
        [Inject] private GameRules _gameRules;

        [SerializeField] private MeshRenderer _meshRenderer; 
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private Rigidbody _rigidbody;

        private IMemoryPool _pool;
        private Transform _transform;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        private int _restFramesCount;
        private bool _isCollidedWithWall;

        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public MeshCollider Collider => _meshCollider;
        public bool IsCollidedWithWall => _isCollidedWithWall;

        private void Awake()
        {
            _transform = transform;
        }

        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            _restFramesCount = 0;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, forceMode);
        }

        public void AddTorque(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddTorque(force, forceMode);
        }

        void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.layer == _layersSettings.GroundLayer)
            {
                _audioManager.PlayGroundChipsHitSound();
            }
            if (collision.gameObject.layer == _layersSettings.WallLayer)
            {
                _isCollidedWithWall = true;
            }
        }

        private void FixedUpdate()
        {
            TrySetKinematicState();
        }

        private void TrySetKinematicState()
        {
            if (_rigidbody.isKinematic) 
                return;

            var slopeAngle = Vector3.Angle(transform.up, Vector3.up);
            if (slopeAngle > _gameRules.AllowedSlopeAngle && slopeAngle < 180 - _gameRules.AllowedSlopeAngle)
                return;

            var position = _transform.position;
            var rotation = _transform.rotation;
            if (_lastPosition == default)
            {
                _lastPosition = position;
                _lastRotation = rotation;
            }
            else
            {
                var isRestPosition = Vector3.SqrMagnitude(_lastPosition - position) < SQR_LAST_POSITION_THRESHOLD;
                var isRestRotation = Quaternion.Angle(_lastRotation, rotation) < LAST_ANGLE_THRESHOLD;
                if (isRestPosition && isRestRotation)
                    _restFramesCount++;
                else
                    _restFramesCount = 0;

                _lastPosition = position;
                _lastRotation = rotation;
            }

            if (_restFramesCount >= WATHCHING_REST_FRAMES)
            {
                _rigidbody.isKinematic = true;
            }
        }

        public void OnDespawned()
        {
            _isCollidedWithWall = false;
            _rigidbody.isKinematic = true;
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _restFramesCount = 0;
            _pool = pool;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Chip>
        {
            private bool _ready;
            private ChipsPool _pool;
            private ChipsSettings _chipsSettings;

            public bool Ready => _ready;
            private int _meshNumber = 0;

            public Factory(DiContainer diContainer, AddressableManager addressableManager, ChipsSettings chipsSettings)
            {
                _chipsSettings = chipsSettings;
                addressableManager.LoadWithCallback<Chip>(prefab =>
                {
                    _ready = true;
                    diContainer.BindMemoryPool<Chip, ChipsPool>()
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(prefab);
                    _pool = diContainer.Resolve<ChipsPool>();
                });
            }

            public override Chip Create()
            {
                if (_ready == false)
                {
                    Debug.LogWarning($"Factory {GetType()} not ready, waiting for load");
                    return null!;
                }

                //это вынести в место создания фишек..
                var chip = _pool.Spawn(_pool);
                chip._meshFilter.sharedMesh = _chipsSettings.ChipsMeshes[_meshNumber];
                chip._meshRenderer.sharedMaterial = _chipsSettings.ChipsMaterial;
                _meshNumber = (_meshNumber + 1) % _chipsSettings.ChipsMeshes.Length;
                return chip;
            }

            private class ChipsPool : MonoPoolableMemoryPool<IMemoryPool, Chip>
            {
            }
        }
    }
}