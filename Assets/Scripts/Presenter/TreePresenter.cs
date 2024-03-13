using System;
using Model;
using View;
using UnityEngine;


namespace Presenter
{
  public class TreePresenter : MonoBehaviourPoolable
  {
    [SerializeField] private Transform _target;
    [SerializeField] private TreeView  _tree_view;

    private readonly TreeModel _tree_model = new TreeModel();

    public event Action<TreePresenter> onFallen;
    public event Action<TreePresenter> onAxedDown;

    public Vector3 position       => transform.position;
    public Vector3 targetPosition => _target.position;


    public void axeHit()
    {
      _tree_model.makeHit();
    }

    protected override void onSpawn()
    {
      gameObject.SetActive( true );
      _tree_model.onTreeStateUpdated += onTreeStateUpdated;
      _tree_model.plant();
    }

    protected override void onDespawn()
    {
      _tree_model.onTreeStateUpdated -= onTreeStateUpdated;
    }

    private void onTreeStateUpdated( TreeState tree_state )
    {
      switch ( tree_state )
      {
        case TreeState.Planted:
          _tree_view.idle();
          break;
        case TreeState.AxeHit:
          _tree_view.shake();
          break;
        case TreeState.AxedDown:
          _tree_view.fall();
          onAxedDown?.Invoke( this );
          break;
        case TreeState.Fallen:
          onFallen?.Invoke( this );
          break;
        default:
          throw new ArgumentOutOfRangeException( nameof( tree_state ), tree_state, null );
      }
    }

    private void Awake()
    {
      _tree_view.tweenRotation.onTweenFinish += () => _tree_model.fallen();
    }
  }
}
