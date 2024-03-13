using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickableBase : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
  #region Public Events
  public event Action<PointerEventData> onClick;
  public event Action<PointerEventData> onPointerDown;
  public event Action<PointerEventData> onPointerEnter;
  public event Action<PointerEventData> onPointerUp;
  public event Action<PointerEventData> onPointerExit;
  #endregion

  #region Unity Methods
  void IPointerDownHandler.OnPointerDown( PointerEventData pointer_data )
  {
    onPointerDown?.Invoke( pointer_data );
  }

  void IPointerUpHandler.OnPointerUp( PointerEventData pointer_data )
  {
    onPointerUp?.Invoke( pointer_data );
  }

  void IPointerClickHandler.OnPointerClick( PointerEventData pointer_data )
  {
    onClick?.Invoke( pointer_data );
  }

  void IPointerEnterHandler.OnPointerEnter( PointerEventData pointer_data )
  {
    onPointerEnter?.Invoke( pointer_data );
  }

  void IPointerExitHandler.OnPointerExit( PointerEventData pointer_data )
  {
    onPointerExit?.Invoke( pointer_data );
  }
  #endregion
}
