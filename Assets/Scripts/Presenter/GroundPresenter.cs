using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;


namespace Presenter
{
  public class GroundPresenter : MonoBehaviour, IGroundPresenter, IFactory<IGroundPresenter>
  {
    [SerializeField] private ClickableBase         _clickable_base;
    [SerializeField] private MeshCollider          _collider;
    [SerializeField] private MonoBehaviourPoolable _tree_asset;

    private readonly List<TreePresenter> _trees_on_ground = new List<TreePresenter>();

    [Inject] private PoolManager _pool;

    private Transform rootTransform => transform.parent;
    private Camera mainCamera { get; set; }

    public event Action onNewTreeAdded;


    private void Awake()
    {
      _clickable_base.onClick += onGroundClick;
      mainCamera = Camera.main;

      addNewTree( new Vector3( -23.14f, 0f, -2.12f ) );
      addNewTree( new Vector3( -21.61f, 0f, -2.12f ) );
      addNewTree( new Vector3( -23.0f, 0f, 3.1f ) );
    }

    private void onTreeFallen( TreePresenter tree )
    {
      tree.onFallen -= onTreeFallen;
      _trees_on_ground.Remove( tree );
      tree.despawn();
    }

    private void onGroundClick( PointerEventData pointer_event_data )
    {
      Ray cam_ray = mainCamera.ScreenPointToRay( pointer_event_data.position );
      if ( !_collider.Raycast( cam_ray, out RaycastHit hit_info, 1000.0f ) )
        return;

      Vector3 new_tree_position = hit_info.point;
      addNewTree( new_tree_position );
    }

    private void addNewTree( Vector3 new_tree_position )
    {
      TreePresenter tree_presenter = _pool.spawnPoolItem<TreePresenter, MonoBehaviourPoolable>( _tree_asset, rootTransform, new_tree_position );
      tree_presenter.onFallen += onTreeFallen;
      _trees_on_ground.Add( tree_presenter );

      onNewTreeAdded?.Invoke();
    }

    public TreePresenter getNearestTree( Vector3 position )
    {
      if ( _trees_on_ground.Count == 0 )
        return null;

      float min_distance = float.MaxValue;
      TreePresenter nearest_tree = null;
      foreach ( TreePresenter tree in _trees_on_ground )
      {
        float distance = Vector3.Distance( position, tree.position );
        if ( distance >= min_distance )
          continue;

        min_distance = distance;
        nearest_tree = tree;
      }

      return nearest_tree;
    }

    public bool anyTreeOnTheGround() => _trees_on_ground.Count > 0;

    public IGroundPresenter Create() => this;
  }

  public interface IGroundPresenter
  {
    event Action  onNewTreeAdded;
    TreePresenter getNearestTree( Vector3 position );
    bool          anyTreeOnTheGround();
  }
}
