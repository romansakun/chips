using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class AddressableManager : IDisposable
    {
         private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _operationHandles = new();

         public async void LoadWithCallback<T> (Action<T> callback) where T : MonoBehaviour
         {
             var instance = await LoadAsync<T>();
             callback(instance);
         }

         public async UniTask<T> LoadAsync<T> () where T : MonoBehaviour
         {
             var prefabType = typeof(T);
             if (_operationHandles.TryGetValue(prefabType.Name, out var handle) == false)
             {
                 handle = Addressables.LoadAssetAsync<GameObject>(prefabType.Name);
                 await handle.Task;

                 if (handle.Status != AsyncOperationStatus.Succeeded)
                     throw new ArgumentException($"Unable to load prefab from {prefabType.Name}\n{handle.OperationException}");

                 _operationHandles.Add(prefabType.Name, handle);
             }
             if (handle.Result == null)
                 throw new ArgumentException($"Unable to load prefab from {prefabType.Name}");

             var component = handle.Result.GetComponent<T>();
             if (component == null)
                 throw new ArgumentException($"Unable to load prefab from {prefabType.Name}");

             return component;
         }

         public void Dispose()
         {
             foreach (var pair in _operationHandles)
             {
                 pair.Value.Release();
             }
             _operationHandles.Clear();
         }

    }
}