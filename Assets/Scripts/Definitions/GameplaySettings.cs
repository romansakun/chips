using System;

namespace Definitions
{
    [Serializable]
    public class GameplaySettings : BaseDef
    {
        public float AllowedScatterRadius  { get; set; }
        public float AllowedSlopeAngle { get; set; }
        public float MaxTimeToWaitHitResult { get; set; }
    }
}