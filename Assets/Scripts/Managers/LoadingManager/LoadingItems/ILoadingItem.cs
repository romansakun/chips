using Cysharp.Threading.Tasks;

namespace Managers
{
    public interface ILoadingItem
    {
        UniTask Load();
    }
}