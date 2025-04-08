using System.Collections.Generic;
using Definitions;
using Gameplay.Battle;
using Gameplay.Chips;
using LogicUtility;
using UnityEngine;

namespace UI.Gameplay
{
    public class GameplayViewModelContext : IContext
    {
        public NpcViewPartModelContext LeftNpcContext { get; } = new();
        public NpcViewPartModelContext RightNpcContext { get; } = new();
        public TimerViewPartModelContext HitTimerContext { get; } = new();
        public ReactiveProperty<bool> IsPlayerCanHitNow { get; } = new();
        public ReactiveProperty<bool> IsHitFailed { get; } = new();
        public ReactiveProperty<bool> IsHitSuccess { get; } = new();
        public List<(Chip, ChipDef)> HitWinningChipsAndDefs { get; } = new();
        public List<(Chip, ChipDef)> HittingChipsAndDefs { get; } = new();
        public List<Chip> HittingChips { get; } = new();
        public PlayerSharedContext HittingPlayer { get; set; }
        public SharedBattleContext Shared { get; set; }
        public Vector3 PlayerHitForce { get; set; }
        public Vector3 PlayerHitTorque { get; set; }
        public bool IsTimeToHitChips { get; set; }
        public bool IsPlayerCannotCollectWinningsChips { get; set; }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;

            LeftNpcContext.Dispose();
            RightNpcContext.Dispose();
            HitTimerContext.Dispose();
            IsHitFailed.Dispose();
            IsHitSuccess.Dispose();
        }

    }
}