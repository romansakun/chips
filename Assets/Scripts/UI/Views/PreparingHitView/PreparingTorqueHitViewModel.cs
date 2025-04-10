namespace UI.PreparingHit
{
    public class PreparingTorqueHitViewModel : PreparingHitViewModel
    {
        private float _value;

        public override void Initialize()
        {
            var range = _gameDefs.PreparingHitSettings.PrepareTorqueRange;
            var infoSprites = _gameDefs.PreparingHitSettings.TorqueInfoSprites;
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
            return _userContext.GetPreparingTorque();
        }

        public override void Dispose()
        {
            _userContext.UpdatePreparedTorque(_value);

            base.Dispose();
        }

    }
}