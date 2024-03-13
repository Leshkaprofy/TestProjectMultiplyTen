using UnityEngine;


namespace View
{
  public class BranchView : MonoBehaviour
  {
    [SerializeField]        private TweenPositionBounce _tween_position_bounce;
    [field: SerializeField] public  TweenPositionTime   tweenPosition { get; private set; }


    public void flyOnBack( Vector3 from, Vector3 to )
    {
      gameObject.SetActive( true );
      tweenPosition.start( from, to );
      _tween_position_bounce.start();
    }

    public void flyFromBack( Vector3 from, Vector3 to )
    {
      tweenPosition.onTweenFinish += onTweenFinish;
      tweenPosition.start( from, to );
      _tween_position_bounce.start();
      return;

      void onTweenFinish()
      {
        tweenPosition.onTweenFinish -= onTweenFinish;
        gameObject.SetActive( false );
      }
    }
  }
}
