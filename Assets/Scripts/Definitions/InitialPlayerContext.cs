using System.Collections.Generic;
using Model;

namespace Definitions
{
    public class InitialPlayerContext : BaseDef
    {
        public Dictionary<string, int> ChipsCount = new Dictionary<string, int>();
        public Dictionary<string, NpcContext> NpcContexts { get; set; } = new Dictionary<string, NpcContext>();
    }
}