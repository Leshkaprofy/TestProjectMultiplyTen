using UnityEngine;

public class GameManager : MonoBehaviour
{
  private void Awake()
  {
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
  }
}
