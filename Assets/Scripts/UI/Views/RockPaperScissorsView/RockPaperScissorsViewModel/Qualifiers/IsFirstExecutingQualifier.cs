namespace UI
{
    public class IsFirstExecutingQualifier : BaseRockPaperScissorsViewModelQualifier
    {
        protected override float Score(RockPaperScissorsViewModelContext context)
        {
            var score = context.WasFirstExecuting ? 0 : 1;
            context.WasFirstExecuting = true;
            return score;
        }
    }
}