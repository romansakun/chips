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
        private readonly Canvas _canvas;
        private readonly DiContainer _diContainer;
        private readonly AddressableManager _addressableManager;
        private readonly RectTransform _canvasRectTransform;
        private readonly List<View> _instancedViews = new();

        public int TopViewSortingOrder => _instancedViews.Count;

        public GuiManager(DiContainer container, AddressableManager addressableManager, Canvas canvas)
        {
            _diContainer = container;
            _addressableManager = addressableManager;
            _canvas = canvas;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        public async UniTask<V> ShowAsync<V,VM> (VM viewModel) where V : View where VM : ViewModel
        {
            var viewPrefab = await _addressableManager.LoadAsync<V>();
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
            viewCanvas.sortingOrder = _instancedViews.Count;
            viewRectTransform.anchoredPosition = Vector2.zero;
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

        public void Close(View view)
        {
            for (int i = _instancedViews.Count - 1; i >= 0; i--)
            {
                var viewInstance = _instancedViews[i];
                if (viewInstance != view) 
                    continue;

                Object.Destroy(viewInstance.gameObject);
                _instancedViews.RemoveAt(i);
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

        public void Dispose()
        {
            CloseAll();
        }

    }
}