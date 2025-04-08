using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Managers
{
    public class AddressableManager : IDisposable
    {
         private readonly Dictionary<string, AsyncOperationHandle<Object>> _objectOperationHandles = new();

         public async UniTask<T> LoadPrefabAsync<T> (string name = null) where T : MonoBehaviour
         {
             var prefabName = name ?? typeof(T).Name;
             var gameObject = await GetOrLoadAsync(prefabName) as GameObject;
             if (gameObject == null)
                 throw new ArgumentException($"Unable to load prefab from {prefabName}. It is not GameObject");

             var component = gameObject.GetComponent<T>();
             if (component == null)
                 throw new ArgumentException($"Unable to load prefab from {prefabName}. It don't contains {typeof(T)}");

             return component;
         }

         public async UniTask<T> LoadAsync<T> (string name) where T : Object
         {
             var obj = await GetOrLoadAsync(name);
             var result = obj as T;
             if (result == null)
                 throw new ArgumentException($"Unable to load Object for {name}. {obj.GetType()} don't match {typeof(T)}");

             return result;
         }

         public async UniTask<Sprite> LoadSpriteAsync (string name, string spriteAtlasName = null)
         {
             if (string.IsNullOrEmpty(spriteAtlasName) == false)
             {
                 var spriteAtlasObject = await GetOrLoadAsync(spriteAtlasName);
                 var spriteAtlas = spriteAtlasObject as SpriteAtlas;
                 if (spriteAtlas == null)
                     throw new ArgumentException($"Unable to load Object for {name}. {spriteAtlasObject.GetType()} don't match {typeof(SpriteAtlas)}");

                 return spriteAtlas.GetSprite(name);
             }
             var result = await GetOrLoadAsync(name);
             var sprite = result as Sprite;
             if (sprite != null)
                 return sprite;

             var texture = result as Texture2D;
             if (texture == null)
                 throw new ArgumentException($"Unable to load Object for {name}. {result.GetType()} don't match {typeof(Sprite)} or {typeof(Texture2D)}");

             sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
             return sprite;
         }

         private async Task<Object> GetOrLoadAsync(string name)
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

             return handle.Result;
         }

         public void Dispose()
         {
             foreach (var pair in _objectOperationHandles)
             {
                 pair.Value.Release();
             }
             _objectOperationHandles.Clear();
         }

    }
}