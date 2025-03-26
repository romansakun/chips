using System;
using UnityEngine;

namespace Definitions
{
    [Serializable]
    public class SelectingChipsForBetSettings : BaseDef
    {
        public float AnimationChipMovingTime { get; set; }

        /// <summary>
        /// Vectors for selected chips for bet
        /// </summary>
        public Vector3 SelectedChipsBetPosition { get; set; } = new Vector3(0, 3, -1);
        public Vector3 SelectedChipsBetRotation { get; set; } = new Vector3(0, 180, 180);
        public Vector3 SelectedChipsBetPositionOffset { get; set; } = new Vector3(0.05f, -0.02f, 0);

        /// <summary>
        /// Vectors for left skipped chips
        /// </summary>
        public Vector3 LeftSkippedChipsPosition { get; set; } = new Vector3(-1.5f, 3, -3);
        public Vector3 LeftSkippedChipsRotation { get; set; } = new Vector3(340, 180, 240);
        public Vector3 LeftSkippedChipsPositionOffset { get; set; } = new Vector3(-.015f, 0, 0);

        /// <summary>
        /// Vectors for right allowed chips
        /// </summary>
        public Vector3 RightChipsPosition { get; set; } = new Vector3(1.5f, 3, -3);
        public Vector3 RightChipsRotation { get; set; } = new Vector3(340, 180, 120);
        public Vector3 RightChipsPositionOffset { get; set; } = new Vector3(.015f, 0, 0);  

        /// <summary>
        /// Vectors for current watching chip
        /// </summary>
        public Vector3 CurrentWatchingChipPosition { get; set; } = new Vector3(0, 3, -3);
        public Vector3 CurrentWatchingChipRotation { get; set; } = new Vector3(340, 180, 180);

        /// <summary>
        /// Canvas objects offsets for watching chips
        /// </summary>
        public Vector2 SelectCurrentChipButtonOffset { get; set; } = new Vector2(0, 220);
        public Vector2 SkipCurrentChipButtonOffset { get; set; } = new Vector2(-220, 0);
        public Vector2 MoveSkippedToWatchingChipButtonOffset { get; set; } = new Vector2(220, 0);
        public Vector2 SkipBetChipButtonOffset { get; set; } = new Vector2(-200, 250);
        public Vector2 BetChipsCountTextOffset { get; set; } = new Vector2(-300, 0);
    }
}