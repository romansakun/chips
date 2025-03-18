namespace Gameplay.Battle
{
    public class IsSetPlayersOrderByRockPaperScissorsStateQualifier : BaseBattleLogicQualifier
    {
        protected override float Score(BattleContext context)
        {
            return context.State == BattleState.SetPlayersOrderByRockPaperScissors ? 1 : 0;
        }
    }
}