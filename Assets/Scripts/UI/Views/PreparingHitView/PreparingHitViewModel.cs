using System.Collections.Generic;
using Definitions;
using Factories;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace UI.PreparingHit
{
    public abstract class PreparingHitViewModel : ViewModel
    {
        [Inject] private AddressableManager _addressableManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private GuiManager _guiManager;
        [Inject] protected UserContextRepository _userContext;
        [Inject] protected GameDefs _gameDefs;

        public IReactiveProperty<Sprite> InfoSprite => _infoSprite;
        public IReactiveProperty<float> ValueScrollbar => _valueScrollbar;
        public IReactiveProperty<string> ValueText => _valueText;

        private ReactiveProperty<Sprite> _infoSprite { get; } = new();
        private ReactiveProperty<float> _valueScrollbar { get; } = new();
        private ReactiveProperty<string> _valueText { get; } = new();

        protected Dictionary<PreparingHitValueState, string> _infoSpriteNames;
        protected float _min;
        protected float _max;

        public override void Initialize()
        {
            var contextValue = GetUserContextValue();
            var scrollbarValue = GetScrollbarValue(contextValue);

            UpdateProperties(scrollbarValue);
        }

        protected abstract void UpdateUserContextValue(float value);
        protected abstract float GetUserContextValue();

        protected virtual string GetValueText(float value)
        {
            return value.ToString("P0");
        }

        public void ProcessSliderValue(float scrollbarValue)
        {
            UpdateProperties(scrollbarValue);

            var needValue = GetNeedValue(scrollbarValue);
            UpdateUserContextValue(needValue);
        }

        public void OnForceButtonClicked()
        {
            var viewModel = _viewModelFactory.Create<PreparingForceHitViewModel>();
            if (_guiManager.TryGetView<PreparingHitView>(out var view))
                view.Initialize(viewModel);
        }

        public void OnTorqueButtonClicked()
        {
            var viewModel = _viewModelFactory.Create<PreparingTorqueHitViewModel>();
            if (_guiManager.TryGetView<PreparingHitView>(out var view))
                view.Initialize(viewModel);
        }

        public void OnAngleButtonClicked()
        {
            var viewModel = _viewModelFactory.Create<PreparingAngleHitViewModel>();
            if (_guiManager.TryGetView<PreparingHitView>(out var view))
                view.Initialize(viewModel);
        }

        public void OnHeightButtonClicked()
        {
            var viewModel = _viewModelFactory.Create<PreparingHeightHitViewModel>();
            if (_guiManager.TryGetView<PreparingHitView>(out var view))
                view.Initialize(viewModel);
        }

        private async void UpdateProperties(float value)
        {
            var valueState = GetScrollbarValueState(value);
            var spriteName = _infoSpriteNames[valueState];
            var sprite = await _addressableManager.LoadSpriteAsync(spriteName, _gameDefs.PreparingHitSettings.InfoSpriteAtlas);
            _infoSprite.Value = sprite;
            _valueScrollbar.Value = value;
            _valueText.Value = GetValueText(value);
        }

        private PreparingHitValueState GetScrollbarValueState(float scrollbarValue)
        {
            var oneThird = (_max - _min) / 3;
            var needValue = GetNeedValue(scrollbarValue);
            var isMinimal = needValue < oneThird + _min;
            var isMaximum = needValue > _max - oneThird;
            return isMinimal
                ? PreparingHitValueState.Minimum
                : isMaximum 
                    ? PreparingHitValueState.Maximum 
                    : PreparingHitValueState.Medium;
        }

        private float GetNeedValue(float scrollbarValue)
        {
            return (_max-_min) * scrollbarValue + _min;
        }

        private float GetScrollbarValue(float value)
        {
            return (value - _min)/(_max-_min);
        }

        public override void Dispose()
        {
            _infoSprite.Dispose();
            _valueScrollbar.Dispose();
            _valueText.Dispose();
        }

    }
}