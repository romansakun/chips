using System.Collections.Generic;

namespace Definitions
{
    public class PreparingHitSettings : BaseDef 
    {
        public float[] PrepareForceRange { get; set; } = { 0.5f, 2.5f };
        public float[] PrepareTorqueRange { get; set; } = { 0.0f, 0.9f };
        public float[] PrepareHeightRange { get; set; } = { 0.1f, 1f };
        public float[] PrepareAngleRange { get; set; } = { 30f, 90f };

        public string InfoSpriteAtlas { get; set; } = "PreparingHitInfoSprites";
        public Dictionary<PreparingHitValueState, string> ForceInfoSprites { get; set; } = new();
        public Dictionary<PreparingHitValueState, string> TorqueInfoSprites { get; set; } = new();
        public Dictionary<PreparingHitValueState, string> HeightInfoSprites { get; set; } = new();
        public Dictionary<PreparingHitValueState, string> AngleInfoSprites { get; set; } = new();
    }
}