using Definitions;

namespace Gameplay.Battle
{
    public class IsSelectingChipsForGameStateQualifier : BaseBattleLogicQualifier
    {
        protected override float Score(BattleContext context)
        {
            return context.State == BattleState.SelectingChipsForGame ? 1 : 0;
        }
    }
}