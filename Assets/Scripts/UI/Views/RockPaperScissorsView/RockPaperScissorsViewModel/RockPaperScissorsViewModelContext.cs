using System.Collections.Generic;
using Definitions;
using Gameplay.Battle;
using LogicUtility;
using UnityEngine;

namespace UI
{
    public class RockPaperScissorsViewModelContext : IContext
    {
        public TimerViewPartModelContext TimerViewPartModelContext { get; } = new();
        public NpcViewPartModelContext LeftNpcViewComponentModelContext { get; } = new();
        public NpcViewPartModelContext RightNpcViewComponentModelContext { get; } = new();
        public ReactiveProperty<Sprite> PlayerChosenHandSprite { get; } = new();
        public ReactiveProperty<Vector3> PlayerChosenHandScale { get; } = new();
        public ReactiveProperty<string> TitleInfoText { get; } = new();
        public ReactiveProperty<bool> TitleInfoTextVisible { get; } = new();
        public ReactiveProperty<string> PlayerInfoText { get; } = new();
        public ReactiveProperty<bool> PlayerInfoTextVisible { get; } = new();
        public ReactiveProperty<bool> PlayerChosenHandVisible { get; } = new();
        public ReactiveProperty<bool> HandButtonsVisible { get; } = new();
        public ReactiveProperty<Sprite> RockButtonSprite { get; } = new();
        public ReactiveProperty<Sprite> PaperButtonSprite { get; } = new();
        public ReactiveProperty<Sprite> ScissorsButtonSprite { get; } = new();

        public SharedBattleContext Shared { get; set; }
        public List<PlayerType> RoundPlayers { get; } = new();
        public Dictionary<PlayerType, RockPaperScissorsHand> RoundPlayersHands { get; } = new ();
        public Dictionary<PlayerType, int> PlayersResults { get; } = new ();
        public bool WasFirstExecuting { get; set; }
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            TimerViewPartModelContext.Dispose();
            LeftNpcViewComponentModelContext.Dispose();
            RightNpcViewComponentModelContext.Dispose();
            PlayerChosenHandSprite.Dispose();
            PlayerChosenHandScale.Dispose();
            PlayerInfoText.Dispose();
            PlayerChosenHandVisible.Dispose();
            RockButtonSprite.Dispose();
            PlayerInfoTextVisible.Dispose();

            IsDisposed = true;
        }
    }
}