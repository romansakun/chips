using System;

namespace Definitions
{
    [Serializable]
    public class GameplaySettings : BaseDef
    {
        public float AllowedScatterRadius  { get; set; }
        public float AllowedSlopeAngle { get; set; }
        public float MaxTimeToWaitHitResult { get; set; }
        public float SqrRestChipPositionThreshold { get; set; }
        public float RestChipAngleThreshold { get; set; }
        public int FramesToWatchRestChip { get; set; }
        public float TimeToShowFailedHit { get; set; }
        public float TimeToMoveWinningChipToPlayer { get; set; }
    }
}