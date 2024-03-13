using System;

public interface IPoolableBasic
{
  event Action onDespawnOneShot;

  void despawn( DespawnType despawn_type = DespawnType.BASE );
}

public interface ISpawnDespawn
{
  void onSpawn();
  void onDespawn( DespawnType despawn_type );
}

public interface IPoolable : IPoolableBasic, ISpawnDespawn
{
  void registerDestroyMethod( Action<DespawnType> destroy_this );
}

#region Public Enum
public enum DespawnType : byte
{
  BASE = 0
, SOFT = 1
, POOL = 2
}
#endregion