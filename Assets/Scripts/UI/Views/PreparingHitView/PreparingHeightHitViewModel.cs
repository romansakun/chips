namespace UI.PreparingHit
{
    public class PreparingHeightHitViewModel : PreparingHitViewModel
    {
        private float _value;

        public override void Initialize()
        {
            var range = _gameDefs.PreparingHitSettings.PrepareHeightRange;
            var infoSprites = _gameDefs.PreparingHitSettings.HeightInfoSprites;
            _min = range[0];
            _max = range[1];
            _infoSpriteNames = infoSprites;

            base.Initialize();
        }

        protected override void UpdateUserContextValue(float value)
        {
            _value = value;
        }

        protected override float GetUserContextValue()
        {
            return _userContext.GetPreparingHeight();
        }

        public override void Dispose()
        {
            _userContext.UpdatePreparedHeight(_value);

            base.Dispose();
        }

    }
}