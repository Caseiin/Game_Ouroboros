using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerStateManager : MonoBehaviour
{
    //Context of the player state  
    PlayerBaseState currentState; //holds a reference to the active state in a state machine

    //Combat state dependecies
    public GameObject melee;
    public Transform aim;
    public GameObject bullet;
    public float firepower = 10f;
    public PlayerAttributes playerAttributes;

    //Dash dependency
    public Slider slider;

    // New instances of the states
    // Initialize states normally (no MonoBehaviour)
    public p_CombatState combatState;

    public p_MovingState movingState = new p_MovingState();

    //Camera
    Camera mainCamera;

    public Vector3 currentdirection;

    void Start()
    {
        //Initialize and pass dependencies 
        combatState = new p_CombatState();
        movingState = new p_MovingState();

        playerAttributes = GetComponent<PlayerAttributes>();
        combatState.Initialize(this);
        movingState.Initialize(this);

        mainCamera = Camera.main;

        // initial player state is the idle state
        currentState = movingState;
        currentState.EnterState(this); //this refers to the context of the player state
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        RotateMeleeTowardsMouse();
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
    
        private void RotateMeleeTowardsMouse()
    {
        if (mainCamera == null || melee == null) return;

        //Get mouse position in world coordinates
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        //Calculate direction from melee to mouse
        currentdirection = (mouseWorldPos - melee.transform.position).normalized;
    // Add minimum distance check to prevent erratic rotation
        if (currentdirection.magnitude < 0.01f) // Adjust threshold as needed
        {
            // Maintain current rotation when mouse is too close
            return;
        }

        // //Calculate rotation angle
        float angle = Mathf.Atan2(currentdirection.y, currentdirection.x) * Mathf.Rad2Deg+90f;

        //Apply rotation to melee object
        aim.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
