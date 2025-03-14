using System;

namespace LogicUtility
{
    public interface IBuildNode
    {
        Type NodeType {get;}
        IBuildNode Next {get; }
        void DirectTo (IBuildNode next);
    }
}