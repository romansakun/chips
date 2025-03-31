using Definitions;

namespace Gameplay.Battle
{
    public class IsChipsBattleStateQualifier : BaseBattleLogicQualifier
    {
        protected override float Score(BattleContext context)
        {
            return context.State == BattleState.ChipsBattle ? 1 : 0; 
        }
    }
}