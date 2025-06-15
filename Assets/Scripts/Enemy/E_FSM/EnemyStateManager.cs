using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, IHealth
{
    public EnemyBaseState currentState; //current context/state  of enemy
    public BaseEnemy enemy;
    //hello
    public e_patrollingState PatrolState;
    public e_ChaseState ChaseState;
    public e_CombatState AttackState;

    public int maxHealth, CurrentHealth = 4;

    SpriteRenderer enemySprite;

    //Animation stuff
    Animator animator;
    private Dictionary<string, float> clipDurations;  // Holds durations of clips by name
    string DeathAnimationName;
    float DeathAnimDuration;

    //LootTable
    [Header("loot")]
    public List<LootItem> lootTable = new List<LootItem>();


    void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Load animation durations dynamically from Animator
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

    public void Start()
    {
        PatrolState = new e_patrollingState();
        currentState = PatrolState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }

    public void TakeHealth()
    {
        CurrentHealth--;
        if (CurrentHealth <= 0)
        {
            Debug.Log("Enemy has died");
            //Dead
            StartCoroutine(DeathRoutine());
        }

        //FlashRed
        StartCoroutine(FlashRed());
    }

    public void GiveHealth()
    {
        Debug.Log("enemy is healing");
    }

    private IEnumerator FlashRed()
    {
        enemySprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = Color.white;
    }


    private IEnumerator DeathRoutine()
    {
        animator.Play(DeathAnimationName);
        yield return new WaitForSeconds(DeathAnimDuration);
        LootDrop();
        Destroy(gameObject);
    }

    void LootDrop()
    {
        foreach (LootItem loot in lootTable)
        {
            if (Random.Range(0f, 100) <= loot.dropChance)
            {
                InstantiateLoot(loot.itemPrefab);
            }

            break;
        }
    }

    private void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

            //just a check
            droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}


// Code reference
// Enemy drop loot logic based from this video: https://www.youtube.com/watch?v=Xe73unMxNiY