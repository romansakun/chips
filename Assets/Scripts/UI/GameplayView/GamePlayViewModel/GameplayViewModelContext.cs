using LogicUtility;

namespace UI
{
    public class GameplayViewModelContext : IContext 
    {
        public bool IsBitButtonPressed { get; set; }
        public bool IsPrepareChipsButtonPressed { get; set; }

        public void Dispose()
        {
         
        }
    }
}