namespace UI.PreparingHit
{
    public class PreparingAngleHitViewModel : PreparingHitViewModel
    {
        public override void Initialize()
        {
            var range = _gameDefs.PreparingHitSettings.PrepareAngleRange;
            var infoSprites = _gameDefs.PreparingHitSettings.AngleInfoSprites;
            _min = range[0];
            _max = range[1];
            _infoSpriteNames = infoSprites;

            base.Initialize();
        }

        protected override void UpdateUserContextValue(float value)
        {
            _userContext.UpdatePreparedAngle(value);
        }

        protected override float GetUserContextValue()
        {
            return _userContext.GetPreparingAngle();
        }

    }
}