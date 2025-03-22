using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Chips;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour 
    {
        [Header("Settings")]
        [SerializeField] private float _padding = 1.0f;
        [SerializeField] private float _smoothTime = 0.3f; 
        [SerializeField] private float _rotationSmoothTime = 3f; 
        [SerializeField] private Vector3 _maxBounds;
        [SerializeField] private Vector3 _minBounds;
        [SerializeField] private Vector3 _rotationBeforeHit;
        [SerializeField] private Vector3 _rotationAfterHit;

        private Camera _camera;
        private List<Chip> _chips;
        private Transform _transform;
        private Tween _cameraRotationTween;
        private Bounds _targetBounds;
        private Vector3 _velocity;
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;

        public Camera Camera => _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _transform = transform;
            _cameraPosition = _transform.position;
            _cameraRotation = _transform.rotation;
        }

        public void FollowByChips(List<Chip> chipsStack)
        {
            _chips = chipsStack;
            _transform.rotation.SetLookRotation(_rotationBeforeHit);
            _cameraRotationTween?.Kill();
            _cameraRotationTween = _transform
                .DORotate(_rotationAfterHit, _rotationSmoothTime)
                .SetEase(Ease.OutCirc);
        }

        public void CancelFollowing()
        {
            _chips = null;
            _cameraRotationTween?.Kill();
        }

        private void LateUpdate()
        {
            if (_chips == null || _chips.Count == 0)
                return;

            CalculateTargetBounds();
            MoveCameraToFitBounds();
        }

        private void CalculateTargetBounds()
        {
            _targetBounds = new Bounds(_chips[0].transform.position, Vector3.zero);
            foreach (var target in _chips)
            {
                _targetBounds.Encapsulate(target.transform.position);
            }
            _targetBounds.Expand(_padding);
        }

        private void MoveCameraToFitBounds()
        {
            if (_targetBounds.size == Vector3.zero)
                return;

            var distance = Mathf.Clamp(_targetBounds.size.magnitude / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad), _minBounds.y, _maxBounds.y);
            var targetPosition = _targetBounds.center - _transform.forward * distance;
            targetPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, _minBounds.x, _maxBounds.x),
                Mathf.Clamp(targetPosition.y, _minBounds.y, _maxBounds.y),
                Mathf.Clamp(targetPosition.z, _minBounds.z, _maxBounds.z));
            transform.position = Vector3.SmoothDamp(
                _transform.position, 
                targetPosition, 
                ref _velocity,
                _smoothTime
            );
        }

        public void ResetPosition()
        {
            _chips = null;
            _transform.position = _cameraPosition;
            _transform.rotation = _cameraRotation;
        }

        private void OnDestroy()
        {
            _cameraRotationTween?.Kill();
        }
    }
}