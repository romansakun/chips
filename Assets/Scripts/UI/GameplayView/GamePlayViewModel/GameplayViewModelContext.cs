using System.Collections.Generic;
using Definitions;
using Gameplay.Battle;
using Gameplay.Chips;
using LogicUtility;

namespace UI
{
    public class GameplayViewModelContext : IContext 
    {
        public ReactiveProperty<bool> ShowHitTimer { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<float> HitTimer { get; set; } = new ReactiveProperty<float>();
        public ReactiveProperty<bool> IsHitFailed { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsHitSuccess { get; set; } = new ReactiveProperty<bool>();
        public List<(Chip, ChipDef)> HitWinningChipsAndDefs { get; set; } = new List<(Chip, ChipDef)>();
        public List<(Chip, ChipDef)> HittingChipsAndDefs { get; set; } = new List<(Chip, ChipDef)>();
        public List<Chip> HittingChips { get; set; } = new List<Chip>();
        //public Dictionary<string, PlayerData> Players { get; set; } = new Dictionary<string, PlayerData>();
        public List<PlayerData> Players { get; set; } = new List<PlayerData>();
        public int HittingPlayerIndex { get; set; }
        public bool IsBitButtonPressed { get; set; }
        public bool IsPrepareChipsButtonPressed { get; set; }
        public bool IsPlayerCannotCollectWinningsChips { get; set; }
        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            IsDisposed = true;
            HitTimer.Dispose();
            ShowHitTimer.Dispose();
            IsHitFailed.Dispose();
            IsHitSuccess.Dispose();
        }

    }
}