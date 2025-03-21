using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Managers
{
    public class AddressableManager : IDisposable
    {
         private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _prefabOperationHandles = new();
         private readonly Dictionary<string, AsyncOperationHandle<Object>> _objectOperationHandles = new();

         public async void LoadPrefabWithCallback<T> (Action<T> callback) where T : MonoBehaviour
         {
             var instance = await LoadPrefabAsync<T>();
             callback(instance);
         }

         public async UniTask<T> LoadPrefabAsync<T> () where T : MonoBehaviour
         {
             var prefabType = typeof(T);
             if (_prefabOperationHandles.TryGetValue(prefabType.Name, out var handle) == false)
             {
                 handle = Addressables.LoadAssetAsync<GameObject>(prefabType.Name);
                 await handle.Task;

                 if (handle.Status != AsyncOperationStatus.Succeeded)
                     throw new ArgumentException($"Unable to load prefab from {prefabType.Name}\n{handle.OperationException}");

                 _prefabOperationHandles.TryAdd(prefabType.Name, handle);
             }
             if (handle.Result == null)
                 throw new ArgumentException($"Unable to load prefab from {prefabType.Name}");

             var component = handle.Result.GetComponent<T>();
             if (component == null)
                 throw new ArgumentException($"Unable to load prefab from {prefabType.Name}");

             return component;
         }

         public async void LoadWithCallback<T> (string name, Action<T> callback) where T : Object
         {
             var instance = await LoadAsync<T>(name);
             callback(instance);
         }

         public async UniTask<T> LoadAsync<T> (string name) where T : Object
         {
             if (_objectOperationHandles.TryGetValue(name, out var handle) == false)
             {
                 handle = Addressables.LoadAssetAsync<Object>(name);
                 await handle.Task;

                 if (handle.Status != AsyncOperationStatus.Succeeded)
                     throw new ArgumentException($"Unable to load Object for {name}\n{handle.OperationException}");

                 _objectOperationHandles.TryAdd(name, handle);
             }
             if (handle.Result == null)
                 throw new ArgumentException($"Unable to load Object for {name}");

             var result = handle.Result as T;
             if (result == null)
                 throw new ArgumentException($"Unable to load Object for {name}");

             return result;
         }

         public void Dispose()
         {
             foreach (var pair in _prefabOperationHandles)
             {
                 pair.Value.Release();
             }
             foreach (var pair in _objectOperationHandles)
             {
                 pair.Value.Release();
             }

             _prefabOperationHandles.Clear();
             _objectOperationHandles.Clear();
         }

    }
}