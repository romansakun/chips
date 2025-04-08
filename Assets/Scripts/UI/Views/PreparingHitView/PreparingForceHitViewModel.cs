namespace UI.PreparingHit
{
    public class PreparingForceHitViewModel : PreparingHitViewModel
    {
        public override void Initialize()
        {
            var range = _gameDefs.PreparingHitSettings.PrepareForceRange;
            var infoSprites = _gameDefs.PreparingHitSettings.ForceInfoSprites;
            _min = range[0];
            _max = range[1];
            _infoSpriteNames = infoSprites;

            base.Initialize();
        }

        protected override void UpdateUserContextValue(float value)
        {
            _userContext.UpdatePreparedForce(value);
        }

        protected override float GetUserContextValue()
        {
            return _userContext.GetPreparingForce();
        }

    }
}