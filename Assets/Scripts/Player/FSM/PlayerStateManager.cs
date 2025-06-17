using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerStateManager : MonoBehaviour
{
    //Context of the player state  
    public PlayerBaseState currentState; //holds a reference to the active state in a state machine


    //Combat state dependecies
    public GameObject melee;
    public Transform aim;
    public GameObject bullet;
    public float firepower = 10f;
    public PlayerAttributes playerAttributes;

    public Transform playerTransfrom;

    //enemy
    public BaseEnemy baseEnemy;

    //Dash dependency
    public Slider slider;


    // New instances of the states
    // Initialize states normally (no MonoBehaviour)
    public p_CombatState combatState;

    public p_MovingState movingState;

    public SpriteRenderer playersprite;

    //Camera
    Camera mainCamera;

    public float footstepspeed = 0.4f;
    public float shootspeed = 0.5f;

    public Vector3 currentdirection;
    GameObject meleeG;

    Animator animator;

    //Player controls
    public PlayerControls controls { get; private set; }
    private Dictionary<string, float> clipDurations;  // Holds durations of clips by name

    string DeathAnimationName;
    float DeathAnimDuration;

    void Awake()
    {
        animator = GetComponent<Animator>();

        clipDurations = new Dictionary<string, float>();
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (!clipDurations.ContainsKey(clip.name))
            {
                clipDurations.Add(clip.name, clip.length);
                Debug.Log($"Clip name: {clip.name}, Length: {clip.length}");

                if (clip.name.ToLower().Contains("death"))
                {
                    Debug.Log($"This clip seems to be a death animation: {clip.name}");
                    DeathAnimationName = clip.name;
                    DeathAnimDuration = clip.length;

                }
            }
        }
    }

    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();


        //Initialize and pass dependencies 
        combatState = new p_CombatState();
        movingState = new p_MovingState();

        playerAttributes = GetComponent<PlayerAttributes>();
        playersprite = GetComponent<SpriteRenderer>();
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
        currentState?.UpdateState(this);
        RotateMeleeTowardsMouse();
    }


    public void SwitchState(PlayerBaseState newState)
    {
        // changes state by taking in new state as a argument 
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }

    public void StartDashCoroutine(IEnumerator coroutine)
    {
        //coroutine needs to be within monobehaviour scripts
        StartCoroutine(coroutine);
    }

    public void StartSwordCoroutine(IEnumerator coroutine)
    {
        //coroutine needs to be within monobehaviour scripts
        StartCoroutine(coroutine);
    }

    public void StartSpeedCoroutine(IEnumerator coroutine)
    {
        //coroutine needs to be within monobehaviour scripts
        StartCoroutine(coroutine);
    }
    #region Mouserotation
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
        float angle = Mathf.Atan2(currentdirection.y, currentdirection.x) * Mathf.Rad2Deg + 90f;

        //Apply rotation to melee object
        aim.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    public void StartCouroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void StopCouroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public IEnumerator DeathRoutine()
    {
        Rigidbody2D player = GetComponent<Rigidbody2D>();
        player.linearVelocity = Vector2.zero;
        animator.Play(DeathAnimationName);
        yield return new WaitForSeconds(DeathAnimDuration);
        // Destroy(gameObject);
        GameObject UI = GameObject.Find("UI");
        UI.SetActive(false);
        SceneManager.LoadScene(4);
    }

    #endregion
}
