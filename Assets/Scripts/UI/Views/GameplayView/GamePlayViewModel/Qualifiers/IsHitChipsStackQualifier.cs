namespace UI.Gameplay
{
    public class IsHitChipsStackQualifier : BaseGameplayViewModelQualifier
    {
        protected override float Score(GameplayViewModelContext context)
        {
            var score = context.IsTimeToHitChips ? 1 : 0;
            context.IsTimeToHitChips = false;
            return score;
        }
    }
}