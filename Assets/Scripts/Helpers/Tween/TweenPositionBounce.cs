using System;
using UnityEngine;
using System.Collections;

public class TweenPositionBounce : MonoBehaviour
{
  [SerializeField] private float _tween_time = 2f;
  [SerializeField] private Vector3 _shift = Vector3.zero;

  public event Action onTweenFinish;


  public void start()
  {
    StartCoroutine( tweenToPositionAndBack() );
  }

  private IEnumerator tweenToPositionAndBack()
  {
    Vector3 start_position = transform.localPosition;
    Vector3 finish_position = start_position + _shift;

    yield return tweenToPosition( start_position, finish_position );
    yield return tweenToPosition( finish_position, start_position );

    onTweenFinish?.Invoke();
  }

  private IEnumerator tweenToPosition( Vector3 start_position, Vector3 finish_position )
  {
    float start_time = Time.time;
    float duration = _tween_time / 2;
    float end_time = start_time + duration;

    while ( Time.time < end_time )
    {
      float time_fraction = (Time.time - start_time) / duration;

      transform.localPosition = Vector3.Lerp( start_position, finish_position, time_fraction );

      yield return null;
    }

    transform.localPosition = finish_position;
  }
}
