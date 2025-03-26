using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class RepeatButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler//, IPointerMoveHandler
    {
        private const float ClickSecondsThreshold = 1f;
        private const float ClickSecondsInterval = .16f; // 6-7 times per second

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _pressedColor;

        private Image _image;
        private RectTransform _rectTransform;
        public event Action OnClick;

        private bool _isPressed;
        private float _pressedTimer;


#if UNITY_EDITOR
        private void OnValidate()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            _defaultColor = _image.color;
        }
#endif

        private void OnDisable()
        {
            _image.color = _defaultColor;
            _pressedTimer = 0;
            _isPressed = false;
        }

        private void Awake()
        {
            _image ??= GetComponent<Image>();
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_isPressed == false)
                return;

            _pressedTimer += Time.deltaTime;
            if (_pressedTimer < ClickSecondsThreshold)
                return;

            if (_pressedTimer < ClickSecondsThreshold + ClickSecondsInterval)
                return;

            _pressedTimer = ClickSecondsThreshold;
            Click();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _image.color = _pressedColor;
            _isPressed = true;
            _pressedTimer = 0f;
        }

        // public void OnPointerMove(PointerEventData eventData)
        // {
        //     if (_isPressed == false)
        //         return;
        //
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, null, out var localPoint);
        //     _isPressed = _rectTransform.rect.Contains(localPoint);
        // }

        public void OnPointerUp(PointerEventData eventData)
        {
            _image.color = _defaultColor;

            if (eventData.dragging == false)
                Click();

            _isPressed = false;
        }

        private void Click()
        {
            OnClick?.Invoke();
        }
    }
}