using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingView : View
    {
        private const float AnimationDuration = 0.25f;

        [SerializeField] private Image _loadingProgressImage;
        [SerializeField] private TextMeshProUGUI _loadingProgressText;

        private int _currentProgress;
        private Sequence _animation;
        private LoadingViewModel _viewModel;

        public override UniTask Initialize(ViewModel viewModel)
        {
            UpdateViewModel(ref _viewModel, viewModel);
            _viewModel.LoadingProgress.Subscribe(OnLoadingProgressChanged);
            return UniTask.CompletedTask;
        }

        private void ViewModelDispose()
        {
            _viewModel.LoadingProgress.Unsubscribe(OnLoadingProgressChanged);
            _viewModel.Dispose();
        }

        private void OnLoadingProgressChanged(int progress)
        {
            _animation?.Kill();
            _animation = DOTween.Sequence()
                .Append(_loadingProgressImage.DOFillAmount(progress / 100f, AnimationDuration))
                .Join(DOTween.To(() => _currentProgress, (x) =>
                {
                    _currentProgress = x;
                    _loadingProgressText.text = $"{x}%";
                }, progress, AnimationDuration))
                .OnComplete(() => _animation = null);
        }

        public bool CanNotBeClosed()
        {
            return _animation != null && _animation.IsPlaying();
        }

        private void OnDestroy()
        {
            _animation?.Kill();
            if (_viewModel != null)
            {
                ViewModelDispose();
            }
        }
    }
}