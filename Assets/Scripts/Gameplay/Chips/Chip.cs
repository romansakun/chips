using System;
using Managers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Chips
{
    public class Chip : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [SerializeField] private MeshRenderer _meshRenderer; 
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private Rigidbody _rigidbody;

        public float SqrSpeedThreshold = 0.05f;
        public float RequiredRestTime = 0.5f;
        
        private static int _groundLayer;

        private IMemoryPool _pool;

        private Transform _transform;
        private IMemoryPool<Chip> _memoryPoolImplementation;
        private float _restTimer;

        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public MeshCollider Collider => _meshCollider;
        public bool IsRest => _restTimer > RequiredRestTime;

        private void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
            _transform = transform;
        }
        
        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            _restTimer = 0f;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, forceMode);
        }
        
        public void AddTorque(Vector3 force, ForceMode forceMode)
        {
            _rigidbody.AddTorque(force, forceMode);
        }

        //private void OnCollisionEnter(Collision other)
        //{
            //Debug.Log($"{gameObject.name} collision ENTER to {other.gameObject.name}");
            // if (other.gameObject.layer == _groundLayer)
            // {
            //     // Получаем "верх" фишки в мировых координатах
            //     Vector3 chipUp = _transform.up;
            //
            //     // Сравниваем с глобальной вертикалью (Vector3.up)
            //     float angle = Vector3.Angle(chipUp, Vector3.up);
            //
            //     if (angle > 5f && angle < (180f - 5f))
            //     {
            //         return;
            //     }
            //     else
            //     {
            //         //_rigidbody.isKinematic = true;
            //         Debug.Log($"{gameObject.name} rigidbody kinematic enabled");
            //     }
            // }
        //}

        // private void OnTriggerEnter(Collider other)
        // {
        //     throw new NotImplementedException();
        // }

        // private void OnCollisionExit(Collision other)
        // {
        //     //Debug.Log($"{gameObject.name} collision EXIT to {other.gameObject.name}");
        // }
        //
        // private void OnCollisionStay(Collision other)
        // {
        //     //Debug.Log($"{gameObject.name} collision STAY to {other.gameObject.name}");
        // }


        // private void FixedUpdate()
        // {
            // Получаем "верх" фишки в мировых координатах
            // Vector3 chipUp = _transform.up;
            //
            // // Сравниваем с глобальной вертикалью (Vector3.up)
            // float angle = Vector3.Angle(chipUp, Vector3.up);
            //
            // if (angle > 30f && angle < (180f - 30f))
            // {
            //     _boxCollider.enabled = false;
            //     _meshCollider.enabled = true;
            //     Debug.Log($"{gameObject.name} meshCollider enabled");
            // }
        //}

        void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.layer != _groundLayer)
                return;
            
            // // Вектор отскока (нормаль к поверхности столкновения)
            // Vector3 bounceDirection = collision.contacts[0].normal;
            //
            // // Добавляем случайный боковой вектор для изменения угла
            // Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * bounceDirection;
            //
            // // Применяем силу отскока
            // _rigidbody.AddForce(randomDirection * 5, ForceMode.Impulse);
            //
            // // Добавляем случайный крутящий момент для вращения
            // _rigidbody.AddTorque(
            //     Random.Range(-1f, 1f) * 10,
            //     Random.Range(-1f, 1f) * 10,
            //     Random.Range(-1f, 1f) * 10,
            //     ForceMode.Impulse
            // );
        }

        private void FixedUpdate()
        {
            if (_rigidbody.isKinematic) 
                return;

            if (_rigidbody.linearVelocity.sqrMagnitude < SqrSpeedThreshold &&
                _rigidbody.angularVelocity.sqrMagnitude < SqrSpeedThreshold) 
            {
                _restTimer += Time.fixedDeltaTime;
            }
            else 
            {
                _restTimer = 0f;
            }
        }

        public void OnDespawned()
        {
            _rigidbody.isKinematic = true;
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Chip>
        {
            private ChipsPool _pool;
            private bool _ready;

            public bool Ready => _ready;

            public Factory(DiContainer diContainer, AddressableManager addressableManager)
            {
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

                return _pool.Spawn(_pool);
            }

            private class ChipsPool : MonoPoolableMemoryPool<IMemoryPool, Chip>
            {
            }
        }
    }
}