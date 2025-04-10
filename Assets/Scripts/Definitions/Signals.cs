using System;
using LogicUtility;
using UI;

namespace Definitions
{
    public struct PlayersBetChipsSetSignal { } 
    public struct PlayersMoveOrderSetSignal { }
    public struct ShowViewSignal
    {
        public View View { get; set; }
    }    
    public struct CloseViewSignal
    {
        public View View { get; set; }
    } 
    public struct LogicAgentCreatedSignal
    {
        public ILogicAgent LogicAgent { get; }

        public LogicAgentCreatedSignal(ILogicAgent logicAgent)
        {
            LogicAgent = logicAgent;
        }
    }

    public struct LogicAgentDisposedSignal
    {
        public ILogicAgent LogicAgent { get; }

        public LogicAgentDisposedSignal(ILogicAgent logicAgent)
        {
            LogicAgent = logicAgent;
        }
    }
}