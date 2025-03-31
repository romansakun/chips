using Definitions;

namespace Gameplay.Battle
{
    public class SetChipsBattleStateAction : BaseBattleLogicAction
    {
        protected override void Execute(BattleContext context)
        {
            context.State = BattleState.ChipsBattle;
        }
    }
}