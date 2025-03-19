using System;

namespace Definitions
{
    [Serializable]
    public class ChipDef : BaseDef
    {
        public string LocalizationKey { get; set; }
        public string Material { get; set; }
        public string Mesh { get; set; }
    }
}