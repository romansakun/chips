using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Managers
{
    public class GuiManager : IDisposable
    {
        private readonly DiContainer _diContainer;
        private readonly AddressableManager _addressableManager;
        private readonly RectTransform _canvasRectTransform;
        private readonly List<View> _instancedViews = new();

        public GuiManager(DiContainer container, AddressableManager addressableManager, Canvas canvas)
        {
            _diContainer = container;
            _addressableManager = addressableManager;
            _canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        public async UniTask<V> ShowAsync<V,VM> (VM viewModel) where V : View where VM : ViewModel
        {
            var viewPrefab = await _addressableManager.LoadPrefabAsync<V>();
            var view = _diContainer.InstantiatePrefabForComponent<V>(viewPrefab, _canvasRectTransform);

            _instancedViews.Add(view);
            PrepareToShow(view);
            view.Initialize(viewModel);

            return view;
        }

        private void PrepareToShow<T>(T viewInstance) where T : View
        {
            var viewCanvas = viewInstance.GetComponent<Canvas>();
            var viewRectTransform = viewInstance.GetComponent<RectTransform>();
            viewRectTransform.anchoredPosition = Vector2.zero;
            viewCanvas.pixelPerfect = true;
            viewCanvas.overrideSorting = true;
            viewCanvas.sortingOrder = viewInstance.OverridedSortingOrder > 0 
                ? viewInstance.OverridedSortingOrder 
                : _instancedViews.Count;
        }

        public bool TryGetView<T>(out T view) where T : View
        {
            view = null;
            for (int i = _instancedViews.Count - 1; i >= 0; i--)
            {
                var viewInstance = _instancedViews[i];
                if (viewInstance is T needView)
                {
                    view = needView;
                    return true;
                }
            }
            return false;
        }

        public void Close<T>(bool onlyTopView = false) where T : View
        {
            for (int i = _instancedViews.Count - 1; i >= 0; i--)
            {
                var viewInstance = _instancedViews[i];
                if (viewInstance is not T)
                    continue;

                Object.Destroy(viewInstance.gameObject);
                _instancedViews.RemoveAt(i);

                if (onlyTopView) 
                    break;
            }
        }

        public void Close(View view, bool onlyTopView = false)
        {
            for (int i = _instancedViews.Count - 1; i >= 0; i--)
            {
                var viewInstance = _instancedViews[i];
                if (viewInstance != view) 
                    continue;

                Object.Destroy(viewInstance.gameObject);
                _instancedViews.RemoveAt(i);

                if (onlyTopView)
                    break;
            }
        }

        public void CloseAll()
        {
            for (int i = _instancedViews.Count - 1; i >= 0; i--)
            {
                var viewInstance = _instancedViews[i];
                if (viewInstance == null) 
                    continue;

                Object.Destroy(viewInstance.gameObject);
            }
            _instancedViews.Clear();
        }

        public Vector2 ScreenPointToLocalPoint(Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRectTransform,
                screenPoint,
                null,
                out var localPoint
            );
            return localPoint;
        }

        public void Dispose()
        {
            CloseAll();
        }

    }
}