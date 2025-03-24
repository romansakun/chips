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

        public ChipFacade Facade { get; private set; }

        private void Awake()
        {
            _transform = transform;
        }

        public void Configure(Action<ChipFacade> configureAction)
        {
            Facade ??= new ChipFacade(this);
            configureAction.Invoke(Facade);
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
            TrySetRestState();
        }

        private void TrySetRestState()
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
            if (this) _pool?.Despawn(this);
        }

        public class ChipFacade
        {
            private readonly Chip _chip;
            public MeshRenderer MeshRenderer { get; private set; }
            public MeshFilter MeshFilter { get; private set; } 
            public MeshCollider MeshCollider { get; private set; } 
            public Rigidbody Rigidbody { get; private set; }
            public Transform Transform { get; private set; } 
            public GameObject GameObject { get; private set; } 

            public ChipFacade(Chip chip)
            {
                _chip = chip;
                MeshRenderer = chip._meshRenderer;
                MeshFilter = chip._meshFilter;
                MeshCollider = chip._meshCollider;
                Rigidbody = chip._rigidbody;
                Transform = chip.transform;
                GameObject = chip.gameObject;
            }

            public void ResetRestFramesCount()
            {
                _chip._restFramesCount = 0;
            }
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, Chip>
        {
        }
    }
}