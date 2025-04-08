using System;

namespace Definitions
{
    [Serializable]
    public class GameplaySettings : BaseDef
    {
        /// <summary>
        /// If the chip is further than the radius from the point of impact, then the move is unsuccessful
        /// </summary>
        public float AllowedScatterRadius  { get; set; }

        /// <summary>
        /// The permissible slope angle of a chip to consider it lying on the surface
        /// </summary>
        public float AllowedSlopeAngle { get; set; }

        /// <summary>
        /// Seconds before the end of the move (when any piece continues to move)
        /// </summary>
        public float MaxTimeToWaitHitResult { get; set; }

        /// <summary>
        /// Seconds before MaxTimeToWaitHitResult
        /// </summary>
        public float FirstTimeToWaitHitResult { get; set; }

        /// <summary>
        /// Permissible difference between current and previous chip position for rest phase check
        /// </summary>
        public float SqrRestChipPositionThreshold { get; set; }

        /// <summary>
        /// Permissible difference between current and previous chip rotation for rest phase check
        /// </summary>
        public float RestChipAngleThreshold { get; set; }

        /// <summary>
        /// The number of frames to check that the chip is not moving and can be set:
        /// Rigidbody.isKinematic = true
        /// </summary>
        public int FramesToWatchRestChip { get; set; }

        /// <summary>
        /// Time to display the message that the move was unsuccessful
        /// </summary>
        public float TimeToShowFailedHit { get; set; }

        /// <summary>
        /// Flight animation time of a won chip
        /// </summary>
        public float TimeToMoveWinningChipToPlayer { get; set; }

        public float Deviation { get; set; } = .05f;
        public float ChipSpacing { get; set; } = .11f;

    }
}