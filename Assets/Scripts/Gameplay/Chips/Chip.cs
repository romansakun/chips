using System;
using Definitions;
using Installers;
using Managers;
using UnityEngine;
using Zenject;

namespace Gameplay.Chips
{
    public class Chip : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [Inject] private AudioManager _audioManager;
        [Inject] private LayersSettings _layersSettings;
        [Inject] private GameDefs _gameDefs;

        [SerializeField] private MeshRenderer _meshRenderer; 
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private Rigidbody _rigidbody;

        private IMemoryPool _pool;
        private Transform _transform;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        private int _restFramesCount;

        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public MeshFilter MeshFilter => _meshFilter;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public MeshCollider Collider => _meshCollider;

        private void Awake()
        {
            _transform = transform;
        }

        public Chip PrepareForHit()
        {
            gameObject.SetActive(true);
            _restFramesCount = 0;
            _rigidbody.isKinematic = false;
            return this;
        }

        public Chip AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            _rigidbody.AddForce(force, forceMode);
            return this;
        }

        public Chip AddTorque(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddTorque(force, forceMode);
            return this;
        }

        private void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.layer == _layersSettings.GroundLayer)
            {
                _audioManager.PlayGroundChipsHitSound();
            }
        }

        private void FixedUpdate()
        {
            TrySetRestMovingState();
        }

        private void TrySetRestMovingState()
        {
            if (_rigidbody.isKinematic) 
                return;

            var slopeAngle = Vector3.Angle(transform.up, Vector3.up);
            var allowedSlopeAngle = _gameDefs.GameplaySettings.AllowedSlopeAngle;
            if (slopeAngle > allowedSlopeAngle && slopeAngle < 180 - allowedSlopeAngle)
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
                var sqrPositionDiff = Vector3.SqrMagnitude(_lastPosition - position);
                var rotationDiff = Quaternion.Angle(_lastRotation, rotation);
                var isRestPosition = sqrPositionDiff < _gameDefs.GameplaySettings.SqrRestChipPositionThreshold;
                var isRestRotation = rotationDiff < _gameDefs.GameplaySettings.RestChipAngleThreshold;
                if (isRestPosition && isRestRotation)
                    _restFramesCount++;
                else
                    _restFramesCount = 0;

                _lastPosition = position;
                _lastRotation = rotation;
            }

            if (_restFramesCount >= _gameDefs.GameplaySettings.FramesToWatchRestChip)
            {
                _rigidbody.isKinematic = true;
            }
        }

        public void OnDespawned()
        {
            _rigidbody.isKinematic = true;
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _rigidbody.isKinematic = true;
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

            public bool Ready => _ready;

            public Factory(DiContainer diContainer, AddressableManager addressableManager)
            {
                addressableManager.LoadPrefabWithCallback<Chip>(prefab =>
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

                var chip = _pool.Spawn(_pool);
                return chip;
            }

            private class ChipsPool : MonoPoolableMemoryPool<IMemoryPool, Chip>
            {
            }
        }
    }
}