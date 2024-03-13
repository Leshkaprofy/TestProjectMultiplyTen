using UnityEngine;

public interface IPoolableComponentInjectFactory
{
  T create<T>( T          prefab, Transform parent = null, Vector3? pos = null ) where T : Component;
  T create<T>( GameObject prefab, Transform parent = null, Vector3? pos = null ) where T : Component;
}