using System;
using UnityEngine;

public class MonoBehaviourPoolable : MonoBehaviour, IPoolable
{
  #region Private Fields
  private Action<DespawnType> _destroy_this;
  #endregion

  #region Public Fields
  public event Action onDespawnOneShot = null;
  #endregion


  #region Protected Methods
  protected virtual void onSpawn() { }

  protected virtual void onDespawn() { }
  #endregion

  #region Public Methods
  public void despawn( DespawnType despawn_type = DespawnType.BASE )
  {
    if ( _destroy_this != null )
      _destroy_this?.Invoke( despawn_type );
  }
  #endregion

  #region interface IPoolable, ISpawnDespawn
  void IPoolable.registerDestroyMethod( Action<DespawnType> destroy_this ) => _destroy_this = destroy_this;

  void ISpawnDespawn.onSpawn()
  {
    onSpawn();
  }

  void ISpawnDespawn.onDespawn( DespawnType despawn_type )
  {
    onDespawn();

    onDespawnOneShot?.Invoke();
    onDespawnOneShot = null;

    gameObject.SetActive( false );
  }
  #endregion
}
