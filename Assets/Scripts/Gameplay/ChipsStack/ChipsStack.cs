using System.Collections.Generic;
using Gameplay.Chips;
using UnityEngine;

namespace Gameplay
{
    
    //лишний класс, надо убрать!
    public class ChipsStack : MonoBehaviour 
    {
        [SerializeField] private List<Chip> _chips;

        private Transform _transform;
        public Transform Transform => transform;

        public List<Chip> Chips => _chips;

        private void Awake()
        {
            _transform = transform;
        }

        public void RemoveAllChips()
        {
            foreach (var chip in _chips)
            {
                if (chip == null)
                    continue;

                chip.Rigidbody.isKinematic = true;
                chip.Dispose();
            }
            _chips.Clear();
        }

        public void AddChip(Chip chip)
        {
            _chips.Add(chip);
        }

        public void Bit(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
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
            var torque = new Vector3(
                Random.Range(-.05f, .05f),
                Random.Range(-.5f, .5f),
                Random.Range(-.05f, .05f));
            
            foreach (var chip in _chips)
            {
               chip.AddForce(force, forceMode);
               chip.AddTorque(torque, ForceMode.Impulse);
            }
        }

        //public int _prewarmFrameCount = 10;

        //private bool _wasBit = false;

        // private void Update()
        // {
        //     if (_prewarmFrameCount > 0)
        //         _prewarmFrameCount--;
        // }
        //
        // private void FixedUpdate()
        // {
        //     if (_prewarmFrameCount > 0)
        //         return;
        //
        //     if (_wasBit)
        //         return;
        //
        //     BitChipsStack();
        // }
        //
        // private void BitChipsStack()
        // {
        //     foreach (var chip in _chips)
        //     {
        //         var chipRigidbody = chip.GetComponent<Rigidbody>();
        //         chipRigidbody.isKinematic = false;
        //         chipRigidbody.AddForce(Vector3.down * 5, ForceMode.Impulse);
        //     }
        //     _wasBit = true;
        // }
    }
}