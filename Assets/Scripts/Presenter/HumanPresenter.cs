using System;
using System.Collections;
using Model;
using UnityEngine;
using View;
using Zenject;


namespace Presenter
{
  public class HumanPresenter : MonoBehaviour
  {
    [SerializeField] private HumanView          _human_view;
    [SerializeField] private TweenPositionSpeed _tween_position;

    [SerializeField] private BranchView _branch1;
    [SerializeField] private BranchView _branch2;
    [SerializeField] private BranchView _branch3;

    [SerializeField] private Transform _pivot_branch1;
    [SerializeField] private Transform _pivot_branch2;
    [SerializeField] private Transform _pivot_branch3;

    private readonly HumanModel    _human_model  = new HumanModel();
    private          TreePresenter _current_tree = null;

    [Inject] private IHousePresenter  _house_presenter;
    [Inject] private IGroundPresenter _ground_presenter;


    private void Start()
    {
      _ground_presenter.onNewTreeAdded += () =>
      {
        if ( _human_model.humanState is HumanState.SearchForTheNearestTree )
          _human_model.walkToTheTree();
      };
      _tween_position.onTweenFinish    += onTweenFinish;
      _human_model.onHumanStateChanged += onHumanStateChanged;
      _human_model.searchForNearestTree();
    }

    private void onHumanStateChanged( HumanState human_state )
    {
      switch ( human_state )
      {
        case HumanState.SearchForTheNearestTree:
          searchForNearestTree();
          break;
        case HumanState.WalkingToTheTree:
          walkToTheTree();
          break;
        case HumanState.Axing:
          axeDownTheTree();
          break;
        case HumanState.GatherBranches:
          gatherBranches();
          break;
        case HumanState.WalkingToTheHouse:
          walkToTheHouse();
          break;
        case HumanState.UnloadingBranches:
          unloadBranches();
          break;
        default:
          throw new ArgumentOutOfRangeException( nameof( human_state ), human_state, null );
      }
    }

    private void searchForNearestTree()
    {
      _current_tree = null;
      _human_view.idle();

      if ( _ground_presenter.anyTreeOnTheGround() )
        _human_model.walkToTheTree();
    }

    private void walkToTheTree()
    {
      _current_tree = _ground_presenter.getNearestTree( transform.position );
      if ( _current_tree is null )
      {
        _human_model.searchForNearestTree();
        return;
      }

      _human_view.walk( _current_tree.targetPosition );
    }

    private void axeDownTheTree()
    {
      Coroutine hits_coroutine = StartCoroutine( hitTheTree() );
      _current_tree.onAxedDown += onTreeAxedDown;
      _human_view.axing( _current_tree.position );
      return;

      void onTreeAxedDown( TreePresenter _ )
      {
        StopCoroutine( hits_coroutine );
        _current_tree.onAxedDown -= onTreeAxedDown;
        _human_model.gatherBranches();
      }

      IEnumerator hitTheTree()
      {
        while ( true )
        {
          yield return new WaitForSeconds( 1.0f );
          _current_tree.axeHit();
        }
      }
    }

    private void gatherBranches()
    {
      _human_view.idle();

      int branches_on_back = 0;
      _branch1.tweenPosition.onTweenFinish += onOneBranchOnBack;
      _branch2.tweenPosition.onTweenFinish += onOneBranchOnBack;
      _branch3.tweenPosition.onTweenFinish += onOneBranchOnBack;

      _branch1.flyOnBack( _current_tree.position, _pivot_branch1.position );
      _branch2.flyOnBack( _current_tree.position, _pivot_branch2.position );
      _branch3.flyOnBack( _current_tree.position, _pivot_branch3.position );
      return;

      void onOneBranchOnBack()
      {
        if ( ++branches_on_back >= 3 )
        {
          _branch1.tweenPosition.onTweenFinish -= onOneBranchOnBack;
          _branch2.tweenPosition.onTweenFinish -= onOneBranchOnBack;
          _branch3.tweenPosition.onTweenFinish -= onOneBranchOnBack;
          _human_model.walkToTheHouse();
        }
      }
    }

    private void walkToTheHouse()
    {
      Vector3 house_position = _house_presenter.frontDoorPosition();
      _human_view.walk( house_position );
    }

    private void unloadBranches()
    {
      _human_view.idle();

      int branches_on_back = 0;
      _branch1.tweenPosition.onTweenFinish += onOneBranchOnBack;
      _branch2.tweenPosition.onTweenFinish += onOneBranchOnBack;
      _branch3.tweenPosition.onTweenFinish += onOneBranchOnBack;

      _branch1.flyFromBack( _pivot_branch1.position, _house_presenter.position() );
      _branch2.flyFromBack( _pivot_branch2.position, _house_presenter.position() );
      _branch3.flyFromBack( _pivot_branch3.position, _house_presenter.position() );
      return;

      void onOneBranchOnBack()
      {
        if ( ++branches_on_back >= 3 )
        {
          _branch1.tweenPosition.onTweenFinish -= onOneBranchOnBack;
          _branch2.tweenPosition.onTweenFinish -= onOneBranchOnBack;
          _branch3.tweenPosition.onTweenFinish -= onOneBranchOnBack;

          _human_model.searchForNearestTree();
        }
      }
    }

    private void onTweenFinish()
    {
      switch ( _human_model.humanState )
      {
        case HumanState.WalkingToTheHouse:
          _human_model.unloadBranches();
          break;
        case HumanState.WalkingToTheTree:
          _human_model.axeDownTheTree();
          break;
      }
    }
  }
}
