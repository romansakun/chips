using System.Collections.Generic;
using Gameplay.Chips;
using LogicUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using Definitions;

namespace UI
{
    public class SelectingFromAllowedChipsViewModelContext : IContext 
    {
        public ReactiveProperty<Vector2> CurrentWatchingChipCanvasPosition { get; } = new ReactiveProperty<Vector2>();
        public ReactiveProperty<Vector2> BetChipCanvasPosition { get; } = new ReactiveProperty<Vector2>();
        public ReactiveProperty<int> BetChipsCount { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> ShowCurrentWatchingChipCount { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> ShowSelectWatchingChipToBetButton { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> ShowSkipCurrentChipButton { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> ShowSkipBetChipButton { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> ShowMoveSkippedToWatchingChipButton { get; } = new ReactiveProperty<bool>();

        public List<(Chip, ChipDef)> AllPlayersChips { get; } = new List<(Chip, ChipDef)>();
        public List<(Chip, ChipDef)> RightSideChips { get; } = new List<(Chip, ChipDef)>();
        public List<(Chip, ChipDef)> LeftSideChips { get; } = new List<(Chip, ChipDef)>();
        public List<(Chip, ChipDef)> BetSelectedChips { get; } = new List<(Chip, ChipDef)>();
        public (Chip, ChipDef) CurrentWatchingChip { get; set; }

        public (DragInputType, PointerEventData) Input { get; set; }
        public Vector2 StartSwipePosition { get; set; }

        public bool IsCurrentWatchingChipSelectedToBet { get; set; }
        public bool IsCurrentWatchingChipSkipped { get; set; }
        public bool IsMoveSkippedToWatchingChip { get; set; }
        public bool IsSkipBetChip { get; set; }

        public bool IsDisposed { get; private set; }


        public void Dispose()
        {
            IsDisposed = true;
            AllPlayersChips.ForEach(c => c.Item1.Dispose());
        }
    }
}