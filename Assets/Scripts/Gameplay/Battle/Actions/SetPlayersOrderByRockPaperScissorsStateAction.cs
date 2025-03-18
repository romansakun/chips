namespace Gameplay.Battle
{
    public class SetPlayersOrderByRockPaperScissorsStateAction : BaseBattleLogicAction
    {
        protected override void Execute(BattleContext context)
        {
            context.State = BattleState.SetPlayersOrderByRockPaperScissors;
        }
    }
}