using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PreparingHitViewPart : MonoBehaviour, IPointerClickHandler
    {
        public GameObject Holder;
        public Image InfoImage;
        public Slider ValueSlider;

        public void Subscribes(PreparingHitViewPartModel viewModel)
        {
            viewModel.InfoSprite.Subscribe(OnInfoSpriteChanged);
            viewModel.ValueSlider.Subscribe(OnValueSliderChanged);
            viewModel.Visible.Subscribe(OnVisibleChanged);

            ValueSlider.onValueChanged.AddListener(viewModel.ProcessSliderValue);
        }

        public void Unsubscribes(PreparingHitViewPartModel viewModel)
        {
            viewModel.InfoSprite.Unsubscribe(OnInfoSpriteChanged);
            viewModel.ValueSlider.Unsubscribe(OnValueSliderChanged);
            viewModel.Visible.Unsubscribe(OnVisibleChanged);

            ValueSlider.onValueChanged.RemoveListener(viewModel.ProcessSliderValue);
        }

        private void OnValueSliderChanged(float value)
        {
            ValueSlider.value = value;
        }

        private void OnInfoSpriteChanged(Sprite infoSprite)
        {
            InfoImage.sprite = infoSprite;
        }

        private void OnVisibleChanged(bool state)
        {
            Holder.SetActive(state);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("PreparingHitViewPart OnPointerClick", this);
        }
    }

    public class PreparingHitViewPartModel
    {
        public IReactiveProperty<Sprite> InfoSprite => _context.InfoSprite;
        public IReactiveProperty<float> ValueSlider => _context.ValueSlider;
        public IReactiveProperty<bool> Visible => _context.Visible;

        private PreparingHitViewPartModelContext _context;

        public Action<float> OnSliderValueChanged { get; set; }

        public void SetContext(PreparingHitViewPartModelContext context)
        {
            _context = context;
        }

        public void ProcessSliderValue(float value)
        {
            OnSliderValueChanged?.Invoke(value);
        }
    }

    public class PreparingHitViewPartModelContext : IDisposable
    {
        public ReactiveProperty<Sprite> InfoSprite { get; } = new();
        public ReactiveProperty<float> ValueSlider { get; } = new();
        public ReactiveProperty<bool> Visible { get; } = new();
        public float NeedValue { get; set; }

        public void Dispose()
        {
            InfoSprite.Dispose();
            ValueSlider.Dispose();
            Visible.Dispose();
        }
    }
}