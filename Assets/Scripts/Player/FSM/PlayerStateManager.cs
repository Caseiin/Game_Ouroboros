using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerStateManager : MonoBehaviour
{
    //Context of the player state

    PlayerBaseState currentState; //holds a reference to the active state in a state machine

    // New instances of the states
    // Initialize states normally (no MonoBehaviour)
    public p_CombatState combatState = new p_CombatState();

    public p_MovingState movingState = new p_MovingState();

    void Start()
    {
        // initial player state is the idle state
        currentState = movingState;
        currentState.EnterState(this); //this refers to the context of the player state
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }


    public void SwitchState(PlayerBaseState newState)
    {
        // changes state by taking in new state as a argument 
        currentState = newState;
        currentState.EnterState(this);
    }

    public void StartDashCoroutine(IEnumerator coroutine)
    {
        //coroutine needs to be within monobehaviour scripts
        StartCoroutine(coroutine);
    }
}
