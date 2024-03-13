using UnityEngine;
using System.Collections;

public class TweenShake : MonoBehaviour
{
  [SerializeField] private float _duration = 1f;
  [SerializeField] private float _magnitude = 0.1f;


  public void start()
  {
    StartCoroutine( shake() );
  }

  private IEnumerator shake()
  {
    Vector3 original_position = transform.position;
    float elapsed = 0.0f;

    while ( elapsed < _duration )
    {
      float x = Random.Range(-1f, 1f) * _magnitude;
      float z = Random.Range(-1f, 1f) * _magnitude;

      transform.position = new Vector3( original_position.x + x, original_position.y, original_position.z + z );

      elapsed += Time.deltaTime;

      yield return null;
    }

    transform.position = original_position;
  }
}
