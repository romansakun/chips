using Definitions;

namespace Gameplay.Battle
{
    public class SetFinishedStateAction : BaseBattleLogicAction
    {
        protected override void Execute(BattleContext context)
        {
            context.State = BattleState.Finished;
        }
    }
}