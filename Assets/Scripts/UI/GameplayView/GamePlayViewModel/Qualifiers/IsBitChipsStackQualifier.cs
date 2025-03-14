namespace UI
{
    public class IsBitChipsStackQualifier : BaseGameplayViewModelQualifier
    {
        protected override float Score(GameplayViewModelContext context)
        {
            var score = context.IsBitButtonPressed ? 1 : 0;
            context.IsBitButtonPressed = false;
            return score;
        }
    }
}