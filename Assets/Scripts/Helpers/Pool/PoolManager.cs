using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Object = UnityEngine.Object;


public class PoolManager
{
  #region Private Fields
  [Inject] private IPoolableComponentInjectFactory _factory;

  private readonly Dictionary<IPoolable, object> _pool_holders = new();

  private Transform _pool_parent;
  #endregion


  #region Public Methods
  public T spawnPoolItem<T, T1>( T1 pool_item, Transform transform_root, Vector3? local_pos = null )
    where T : T1
    where T1 : MonoBehaviour, IPoolable
  {
    if ( !pool_item )
      return null;

    return getOrCreatePoolHolder( pool_item ).spawnPoolItem<T>( transform_root, local_pos );
  }

  public T spawnPoolItem<T>( T pool_item, Transform transform_root, Vector3? local_pos = null )
    where T : MonoBehaviour, IPoolable
    => spawnPoolItem<T, T>( pool_item, transform_root, local_pos );

  public void despawn<T>( T pool_item, DespawnType despawn_type )
    where T : MonoBehaviour, IPoolable
  {
    if ( !pool_item )
      return;

    getOrCreatePoolHolder( pool_item ).despawn( pool_item, despawn_type );
  }

  public void destroy<T>( T pool_item )
    where T : MonoBehaviour, IPoolable
  {
    PoolHolder<T> pool_holder = getPoolHolder( pool_item );
    if ( pool_holder != null )
    {
      pool_holder.destroy( pool_item );
      return;
    }

    Object.Destroy( pool_item.gameObject );
  }

  public void destroyPoolHolder<T>( T pool_item, bool only_pool_item_free = true )
    where T : MonoBehaviour, IPoolable
    => destroyPoolHolder( getPoolHolder( pool_item ), only_pool_item_free );

  public void destroy<T>( bool only_pool_item_free = true )
    where T : MonoBehaviour, IPoolable
  {
    foreach ( PoolHolder<T> it in getFreeHolders<T>().ToArray() )
      destroyPoolHolder( it, only_pool_item_free );
  }

  public IEnumerable<PoolHolder<T>> getFreeHolders<T>()
    where T : MonoBehaviour, IPoolable
    => _pool_holders.Values.OfType<PoolHolder<T>>().Where( a => a.isAllItemFree() );

  public PoolHolder<T> getOrCreatePoolHolder<T>( T pool_item )
    where T : MonoBehaviour, IPoolable
    => getPoolHolder( pool_item ) ?? createPoolHolder( pool_item );
  #endregion

  #region Private Methods
  private void destroyPoolHolder<T>( PoolHolder<T> pool_holder, bool only_pool_item_free = true )
    where T : MonoBehaviour, IPoolable
  {
    if ( pool_holder == null )
      return;

    pool_holder.destroyAllItem( only_pool_item_free );

    if ( !only_pool_item_free )
    {
      _pool_holders.Remove( pool_holder.poolItem );
      if ( pool_holder.poolTransform )
        Object.Destroy( pool_holder.poolTransform.gameObject );
    }
  }

  private PoolHolder<T> getPoolHolder<T>( T pool_item )
    where T : MonoBehaviour, IPoolable
  {
    if ( !pool_item )
      throw new ArgumentNullException( nameof( pool_item ) );

    if ( _pool_holders.TryGetValue( pool_item, out object pool_holder ) )
      return pool_holder as PoolHolder<T>;

    return null;
  }

  private PoolHolder<T> createPoolHolder<T>( T pool_item )
    where T : MonoBehaviour, IPoolable
  {
    if ( !pool_item )
      throw new ArgumentNullException( nameof( pool_item ) );

    if ( !_pool_parent )
      _pool_parent = new GameObject( nameof( PoolManager ) ).transform;

    PoolHolder<T> new_pool_holder = new PoolHolder<T>( pool_item, _pool_parent, _factory );

    _pool_holders.Add( pool_item, new_pool_holder );

    return new_pool_holder;
  }
  #endregion
}