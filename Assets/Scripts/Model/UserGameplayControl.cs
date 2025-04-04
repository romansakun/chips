using System;

namespace Model
{
    [Serializable]
    public class UserGameplayControl
    {
        public float PreparedForce { get; set; }
        public float PreparedTorque { get; set; }
        public float PreparedAngle { get; set; }
        public float PreparedHeight { get; set; }
    }
}