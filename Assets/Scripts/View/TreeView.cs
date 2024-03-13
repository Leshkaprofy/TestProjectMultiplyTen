using UnityEngine;


namespace View
{
  public class TreeView : MonoBehaviour
  {
    [SerializeField]        private TweenShake    _tween_shake;
    [field: SerializeField] public  TweenRotation tweenRotation { get; private set; }


    public void idle()
    {
      transform.LookAt( Vector3.zero );
    }

    public void shake()
    {
      _tween_shake.start();
    }

    public void fall()
    {
      tweenRotation.start( Quaternion.Euler( 80, 0, 0 ) );
    }
  }
}
