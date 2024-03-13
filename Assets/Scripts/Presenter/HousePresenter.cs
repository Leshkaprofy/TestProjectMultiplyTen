using UnityEngine;
using Zenject;


namespace Presenter
{
  public class HousePresenter : MonoBehaviour, IHousePresenter, IFactory<IHousePresenter>
  {
    [SerializeField] private Transform _front_door_pivot;


    public Vector3 frontDoorPosition()
    {
      return _front_door_pivot.position;
    }

    public Vector3 position()
    {
      return transform.position;
    }

    public IHousePresenter Create() => this;
  }

  public interface IHousePresenter
  {
    Vector3 frontDoorPosition();
    Vector3 position();
  }
}
