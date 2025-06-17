using UnityEngine;

public abstract class PlayerBaseState
{
    //Abstract or UI of the player state
    // Start is called once before the first execution of Update after the MonoBehaviour is created

  public  abstract void EnterState(PlayerStateManager playerState);

  public abstract void UpdateState(PlayerStateManager playerState);

  public  abstract void ExitState(PlayerStateManager playerState);
}
