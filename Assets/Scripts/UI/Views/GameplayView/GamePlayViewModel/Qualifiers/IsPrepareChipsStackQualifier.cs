namespace UI.Gameplay
{
    public class IsPrepareChipsStackQualifier : BaseGameplayViewModelQualifier
    {
        protected override float Score(GameplayViewModelContext context)
        {
            var score = context.IsPrepareChipsButtonPressed ? 1 : 0;
            context.IsPrepareChipsButtonPressed = false;
            return score;
        }
    }
}