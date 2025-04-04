using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimerViewPart : MonoBehaviour
    {
        public GameObject Holder;
        public Image CircleImage;
        public TextMeshProUGUI TimerText;
        public RectTransform CircleRectTransform;
        public RectTransform TimerRectTransform;

        public void Subscribes(TimerViewPartModel viewModel)
        {
            viewModel.Color.Subscribe(OnColorChanged);
            viewModel.Scale.Subscribe(OnScaleChanged);
            viewModel.TimerText.Subscribe(OnTimerTextChanged);
            viewModel.TimerText.Subscribe(OnTimerTextChanged);
            viewModel.Visible.Subscribe(OnVisibleChanged);
        }

        public void Unsubscribes(TimerViewPartModel viewModel)
        {
            viewModel.Color.Unsubscribe(OnColorChanged);
            viewModel.Scale.Unsubscribe(OnScaleChanged);
            viewModel.TimerText.Unsubscribe(OnTimerTextChanged);
            viewModel.TimerText.Unsubscribe(OnTimerTextChanged);
            viewModel.Visible.Unsubscribe(OnVisibleChanged);
        }

        private void OnColorChanged(Color color)
        {
            CircleImage.color = color;
            TimerText.color = color;
        }

        private void OnScaleChanged(Vector3 scale)
        {
            CircleRectTransform.localScale = scale;
            TimerRectTransform.localScale = scale;
        }

        private void OnTimerTextChanged(string info)
        {
            TimerText.text = info;
        }

        private void OnVisibleChanged(bool state)
        {
            Holder.SetActive(state);
        }
    }

    public class TimerViewPartModel
    {
        private TimerViewPartModelContext _context;
        public IReactiveProperty<Color> Color => _context.Color;
        public IReactiveProperty<Vector3> Scale => _context.Scale;
        public IReactiveProperty<string> TimerText => _context.TimerText;
        public IReactiveProperty<bool> Visible => _context.Visible;

        public void SetContext(TimerViewPartModelContext context)
        {
            _context = context;
        }
    }

    public class TimerViewPartModelContext : IDisposable
    {
        public ReactiveProperty<Color> Color { get; } = new();
        public ReactiveProperty<Vector3> Scale { get; } = new(Vector3.one);
        public ReactiveProperty<string> TimerText { get; } = new();
        public ReactiveProperty<bool> Visible { get; } = new();

        public void Dispose()
        {
            Color.Dispose();
            Scale.Dispose();
            TimerText.Dispose();
            Visible.Dispose();
        }
    }
}