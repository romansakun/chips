using Cysharp.Threading.Tasks;

namespace Managers
{
    public class FakeLoadingItem : ILoadingItem
    {
        private readonly int _delay;

        public FakeLoadingItem(int delay = 750)
        {
            _delay = delay;
        }

        public async UniTask Load()
        {
            await UniTask.Delay(_delay);
        }
    }
}