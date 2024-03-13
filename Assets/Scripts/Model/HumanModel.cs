using System;


namespace Model
{
  public class HumanModel
  {
    public HumanState               humanState { get; private set; }
    public event Action<HumanState> onHumanStateChanged;


    public void searchForNearestTree()
    {
      setHumanState( HumanState.SearchForTheNearestTree );
    }

    public void walkToTheTree()
    {
      setHumanState( HumanState.WalkingToTheTree );
    }

    public void axeDownTheTree()
    {
      setHumanState( HumanState.Axing );
    }

    public void gatherBranches()
    {
      setHumanState( HumanState.GatherBranches );
    }

    public void walkToTheHouse()
    {
      setHumanState( HumanState.WalkingToTheHouse );
    }

    public void unloadBranches()
    {
      setHumanState( HumanState.UnloadingBranches );
    }

    private void setHumanState( HumanState human_state )
    {
      HumanState prev_state = humanState;
      humanState = human_state;

      if ( prev_state != human_state )
        onHumanStateChanged?.Invoke( human_state );
    }
  }

  public enum HumanState
  {
    SearchForTheNearestTree = 1
  , WalkingToTheTree
  , Axing
  , GatherBranches
  , WalkingToTheHouse
  , UnloadingBranches
  };
}
