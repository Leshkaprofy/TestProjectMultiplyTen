using System;
using UnityEngine;
using System.Collections;

public class TweenRotation : MonoBehaviour
{
  [SerializeField] private float _tween_seconds = 1f;

  public event Action onTweenFinish;


  public void start( Quaternion finish_rotation )
  {
    StartCoroutine( tweenToRotation( finish_rotation ) );
  }

  private IEnumerator tweenToRotation( Quaternion finish_rotation )
  {
    Quaternion start_rotation = transform.rotation;
    float start_time = Time.time;
    float end_time = start_time + _tween_seconds;

    while ( Time.time < end_time )
    {
      float time_fraction = (Time.time - start_time) / _tween_seconds;

      transform.rotation = Quaternion.Lerp( start_rotation, finish_rotation, time_fraction );

      yield return null;
    }

    transform.rotation = finish_rotation;
    onTweenFinish?.Invoke();
  }
}
