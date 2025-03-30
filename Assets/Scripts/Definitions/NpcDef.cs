using System;
using System.Collections.Generic;

namespace Definitions
{
    [Serializable]
    public class NpcDef : BaseDef
    {
        public string NickLocalizationKey { get; set; }
        public string AvatarSprite { get; set; }
        public Dictionary<string, int> ChipsCount = new Dictionary<string, int>();
    }
}