using System;


namespace Model
{
  public class TreeModel
  {
    public TreeState treeState { get; private set; }

    public event Action<TreeState> onTreeStateUpdated;

    private int hitCount { get; set; }
    private bool isAxedDown => hitCount >= AXE_HIT_TO_DOWN;

    private static readonly int AXE_HIT_TO_DOWN = 3;


    public void plant()
    {
      setState( TreeState.Planted );
      hitCount = 0;
    }

    public void makeHit()
    {
      hitCount++;
      setState( isAxedDown ? TreeState.AxedDown : TreeState.AxeHit );
    }

    public void fallen()
    {
      setState( TreeState.Fallen );
    }

    private void setState( TreeState new_state )
    {
      treeState = new_state;
      onTreeStateUpdated?.Invoke( new_state );
    }
  }

  public enum TreeState
  {
    Planted
  , AxeHit
  , AxedDown
  , Fallen
  }
}
