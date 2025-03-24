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
        public bool IsDisposed { get; private set; }
        public List<Chip> AllPlayersChips { get; } = new List<Chip>();
        public List<Chip> RightSideChips { get; } = new List<Chip>();
        public List<Chip> LeftSideChips { get; } = new List<Chip>();
        public List<Chip> SelectedChips { get; } = new List<Chip>();

        public (InputType, PointerEventData) Input { get; set; }
        public Vector2 StartSwipePosition { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
            AllPlayersChips.ForEach(c => c.Dispose());
            AllPlayersChips.Clear();
        }
    }
}