using System;
using System.Collections.Generic;

namespace Definitions
{
    [Serializable]
    public class LocalizationDef : BaseDef 
    {
        public string Description { get; set; }
        public Dictionary<string, string> LocalizationText { get; set; } = new Dictionary<string, string>();
    }
}