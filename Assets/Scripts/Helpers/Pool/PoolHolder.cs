using System;
using UnityEngine;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


public class PoolHolder<TPoolableItem>
  where TPoolableItem : MonoBehaviour, IPoolable
{
  #region Private Fields
  private List<TPoolableItem> _all_pool_items  = new();
  private List<TPoolableItem> _free_pool_items = new();

  private readonly IPoolableComponentInjectFactory _factory;
  #endregion

  #region Public Fields
  public TPoolableItem poolItem { get; }

  public Transform poolTransform { get; }

  private int poolItemCount     => _all_pool_items.Count;
  private int poolItemFreeCount => _free_pool_items.Count;
  #endregion


  #region Public Methods
  public PoolHolder( TPoolableItem pool_item, Transform pool_transform, IPoolableComponentInjectFactory factory )
  {
    poolItem      = pool_item;
    poolTransform = pool_transform;
    _factory      = factory;
  }

  public T spawnPoolItem<T>( Transform transform_root, Vector3? local_pos = null )
  {
    TPoolableItem pool_item = _free_pool_items.lastOrDefault();
    if ( pool_item )
    {
      _free_pool_items.Remove( pool_item );
      pool_item.transform.SetParent( transform_root, false );
    } else
    {
      pool_item = instantiatePoolItem( transform_root );
    }

    if ( local_pos.HasValue )
      pool_item.transform.localPosition = local_pos.Value;

    pool_item.onSpawn();
    if ( pool_item is T item )
      return item;

    throw new InvalidCastException( $"{nameof( pool_item )} is not {typeof( T ).Name}, but {pool_item.GetType().Name}" );
  }

  public void despawn( TPoolableItem pool_item, DespawnType despawn_type )
  {
    if ( _free_pool_items.Contains( pool_item ) )
      return;

    _free_pool_items.Add( pool_item );
    pool_item.onDespawn( despawn_type );

    if ( despawn_type == DespawnType.POOL )
      pool_item.transform.SetParent( poolTransform, false );
  }

  public void remove( TPoolableItem pool_item )
  {
    _all_pool_items.Remove( pool_item );
    _free_pool_items.Remove( pool_item );
  }

  public void destroyAllItem( bool only_pool_item_free )
  {
    void destroyAllItem( List<TPoolableItem> pool_items )
    {
      while ( pool_items.Count > 0 )
        destroy( pool_items[0] );

      pool_items.Clear();
    }

    destroyAllItem( only_pool_item_free ? _free_pool_items : _all_pool_items );
  }

  public void destroy( TPoolableItem pool_item )
  {
    if ( !pool_item )
    {
      Debug.LogWarning( $"{this} pool_item is NULL" );
      return;
    }

    remove( pool_item );

    if ( !_free_pool_items.Contains( pool_item ) )
      pool_item.onDespawn( DespawnType.SOFT );

    Object.Destroy( pool_item.gameObject );
  }

  public bool isAllItemFree()
  {
    if ( poolItem == null )
      return false;

    return poolItemCount == poolItemFreeCount;
  }
  #endregion

  #region Private Methods
  private TPoolableItem instantiatePoolItem( Transform transform_root, Vector3? pos = null )
  {
    TPoolableItem poolable_item = _factory.create( poolItem, transform_root, pos );
    poolable_item.registerDestroyMethod( despawn_type => despawn( poolable_item, despawn_type ) );

    _all_pool_items.Add( poolable_item );

    return poolable_item;
  }
  #endregion
}