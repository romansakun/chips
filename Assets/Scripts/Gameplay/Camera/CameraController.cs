using System;
using System.Collections.Generic;
using Gameplay.Chips;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour 
    {
        [Header("Настройки")]
        public float padding = 1.0f; // Отступ от краев
        public float smoothTime = 0.3f; // Плавность движения

        [SerializeField] private Camera _camera;
        public Camera Camera => _camera;

        private Transform _transform;
        private Bounds _targetBounds;
        private Vector3 _velocity;
        private List<Chip> _chips;
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;

        private void Awake()
        {
            _transform = transform;
            if (_camera == null)
                _camera = GetComponent<Camera>();

            _cameraPosition = _transform.position;
            _cameraRotation = _transform.rotation;
        }

        public void FollowByChips(List<Chip> chipsStack)
        {
            _chips = chipsStack;
        }

        private void LateUpdate()
        {
            if (_chips == null || _chips.Count == 0)
                return;
            
            // 2. Рассчитать общие границы
            CalculateTargetBounds();
            
            // 3. Позиционировать камеру
            MoveCameraToFitBounds();
        }

        private void CalculateTargetBounds()
        {
            _targetBounds = new Bounds();

            // Инициализируем границы первой фишкой
            _targetBounds = new Bounds(_chips[0].transform.position, Vector3.zero);
            
            // Расширяем границы для всех фишек
            foreach (var target in _chips)
            {
                _targetBounds.Encapsulate(target.transform.position);
            }
            
            // Добавляем отступ
            _targetBounds.Expand(padding);
        }

        private void MoveCameraToFitBounds()
        {
            if (_targetBounds.size == Vector3.zero) return;

            // Центр границ
            Vector3 center = _targetBounds.center;
            
            // Для ортографической камеры
            // if (Camera.main.orthographic)
            // {
            //     // Рассчитываем размер камеры
            //     float screenRatio = (float)Screen.width / Screen.height;
            //     float boundsRatio = _targetBounds.size.x / _targetBounds.size.z;
            //     
            //     float size = Mathf.Max(_targetBounds.size.x / (2 * screenRatio), 
            //                          _targetBounds.size.z / 2);
            //                          
            //     Camera.main.orthographicSize = Mathf.Lerp(
            //         Camera.main.orthographicSize, 
            //         size + padding, 
            //         smoothTime * Time.deltaTime
            //     );
            //     
            //     // Позиция камеры над центром
            //     Vector3 targetPosition = new Vector3(
            //         center.x,
            //         transform.position.y, // Сохраняем высоту
            //         center.z - 5 // Смещение по Z
            //     );
            //     
            //     transform.position = Vector3.SmoothDamp(
            //         transform.position, 
            //         targetPosition, 
            //         ref _velocity, 
            //         smoothTime
            //     );
            // }
            // // Для перспективной камеры (3D)
            // else
            {
                // Рассчитываем расстояние до камеры
                float distance = Mathf.Clamp(_targetBounds.size.magnitude / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad), 10f, 1000f);

                // Позиция камеры
                Vector3 targetPosition = center - _transform.forward * distance;
                transform.position = Vector3.SmoothDamp(
                    _transform.position, 
                    targetPosition, 
                    ref _velocity, 
                    smoothTime
                );
            }
        }

        public void ResetPosition()
        {
            _chips = null;
            _transform.position = _cameraPosition;
            _transform.rotation = _cameraRotation;
        }
    }
}