using Cysharp.Threading.Tasks;
using Model;
using Zenject;

namespace Managers
{
    public class UserContextLoadingItem : ILoadingItem
    {
        [Inject] private UserContextRepository _userContext;

        public async UniTask Load()
        {
            if (_userContext.TryLoadingPlayerContext() == false)
            {
                _userContext.CreateNewPlayerContext();
            }
            await UniTask.Yield();
        }
    }
}