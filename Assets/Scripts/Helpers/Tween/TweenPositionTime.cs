using System;
using UnityEngine;
using System.Collections;

public class TweenPositionTime : MonoBehaviour
{
  [SerializeField] private float _tween_time = 2f;

  public event Action onTweenFinish;


  public void start( Vector3 start_position, Vector3 finish_position )
  {
    StartCoroutine( tweenToPosition( start_position, finish_position ) );
  }

  private IEnumerator tweenToPosition( Vector3 start_position, Vector3 finish_position )
  {
    transform.position = start_position;
    float start_time = Time.time;
    float end_time = start_time + _tween_time;

    while ( Time.time < end_time )
    {
      float time_fraction = (Time.time - start_time) / _tween_time;

      transform.position = Vector3.Lerp( start_position, finish_position, time_fraction );

      yield return null;
    }

    transform.position = finish_position;
    onTweenFinish?.Invoke();
  }
}
