using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class TimerViewBit
    {
        public GameObject Holder;
        public Image CircleImage;
        public TextMeshProUGUI TimerText;
        public RectTransform CircleRectTransform;
        public RectTransform TimerRectTransform;

        public void Subscribes(TimerViewBitModel viewModel)
        {
            viewModel.Color.Subscribe(OnColorChanged);
            viewModel.Scale.Subscribe(OnScaleChanged);
            viewModel.TimerText.Subscribe(OnTimerTextChanged);
            viewModel.Visible.Subscribe(OnVisibleChanged);
        }

        public void Unsubscribes(TimerViewBitModel viewModel)
        {
            viewModel.Color.Unsubscribe(OnColorChanged);
            viewModel.Scale.Unsubscribe(OnScaleChanged);
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

    public class TimerViewBitModel
    {
        private TimerViewBitModelContext _context;
        public IReactiveProperty<Color> Color => _context.Color;
        public IReactiveProperty<Vector3> Scale => _context.Scale;
        public IReactiveProperty<string> TimerText => _context.TimerText;
        public IReactiveProperty<bool> Visible => _context.Visible;

        public void SetContext(TimerViewBitModelContext context)
        {
            _context = context;
        }
    }

    public class TimerViewBitModelContext : IDisposable
    {
        public ReactiveProperty<Color> Color { get; } = new ReactiveProperty<Color>();
        public ReactiveProperty<Vector3> Scale { get; } = new ReactiveProperty<Vector3>();
        public ReactiveProperty<string> TimerText { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> Visible { get; } = new ReactiveProperty<bool>();

        public void Dispose()
        {
            Color.Dispose();
            Scale.Dispose();
            TimerText.Dispose();
            Visible.Dispose();
        }
    }
}