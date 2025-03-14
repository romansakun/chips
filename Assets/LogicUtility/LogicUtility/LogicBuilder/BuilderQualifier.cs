using System;

namespace LogicUtility
{
    public class BuilderQualifier : IBuildNode
    {
        public Type NodeType { get; }
        public IBuildNode Next { get; private set; }

        public BuilderQualifier(Type qualifierType)
        {
            NodeType = qualifierType;
        }

        public void DirectTo (IBuildNode next)
        {
            Next = next;
        }
    }
}