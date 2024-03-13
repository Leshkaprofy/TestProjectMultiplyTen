using UnityEngine;

public class PoolableComponentInjectFactory : IPoolableComponentInjectFactory
{
  public T create<T>( T prefab, Transform parent = null, Vector3? pos = null )
    where T : Component
  {
    return ContainerHolder.container.InstantiatePrefab( prefab, parent, pos ).GetComponent<T>();
  }

  public T create<T>( GameObject prefab, Transform parent = null, Vector3? pos = null )
    where T : Component
  {
    T res = ContainerHolder.container.InstantiatePrefab( prefab, parent ).GetComponent<T>();
    if ( pos.HasValue )
      res.transform.position = pos.Value;
    return res;
  }
}