using UnityEngine;


namespace View
{
  public class HumanView : MonoBehaviour
  {
    [SerializeField] private Animator           _animator;
    [SerializeField] private TweenPositionSpeed _tween_position;

    private static readonly int _is_idle      = Animator.StringToHash( "isIdle" );
    private static readonly int _is_walk      = Animator.StringToHash( "isWalk" );
    private static readonly int _is_axing     = Animator.StringToHash( "isAxing" );
    private static readonly int _is_gathering = Animator.StringToHash( "isGathering" );

    private Transform cachedTransform { get; set; }


    private void Awake()
    {
      cachedTransform = transform;

      if ( !_animator.isInitialized )
        _animator.Rebind();
    }

    public void axing( Vector3 tree_position )
    {
      cachedTransform.LookAt( tree_position );
      _animator.SetBool( _is_axing, true );
    }

    public void idle()
    {
      _animator.SetBool( _is_idle, true );
    }

    public void gathering()
    {
      _animator.SetBool( _is_gathering, true );
    }

    public void walk( Vector3 finish_position )
    {
      _animator.SetBool( _is_walk, true );
      cachedTransform.LookAt( finish_position );
      _tween_position.start( cachedTransform.position, finish_position );
    }
  }
}
