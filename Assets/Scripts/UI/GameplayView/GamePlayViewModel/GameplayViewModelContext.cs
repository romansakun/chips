using LogicUtility;

namespace UI
{
    public class GameplayViewModelContext : IContext 
    {
        public ReactiveProperty<bool> ShowHitTimer { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<float> HitTimer { get; set; } = new ReactiveProperty<float>();
        public bool IsBitButtonPressed { get; set; }
        public bool IsPrepareChipsButtonPressed { get; set; }
        public bool IsPlayerCannotCollectWinningsChips { get; set; }
        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            HitTimer.Dispose();
            ShowHitTimer.Dispose();
            IsDisposed = true;
        }

    }
}