using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    private Animator animator;

    // Melee variables
    public GameObject Melee;
    bool isAttacking = false;
    float atkDuration = 0.3f;
    float atkTimer = 0f;

    // Ranged variables
    public Transform aim;
    public GameObject bullet;
    public float firepower = 10f;
    float shootCoolDown = 0.25f;
    float shootTimer = 1f;

    // Added direction storage
    private Vector2 currentAimDirection;


    //Enemy variables
    private EnemyCombat1 enemyCombat;

    //Combat sound effects
    public AudioClip[] clips;
    private AudioSource audioSource;

    //Player stats variables
    private PlayerAttributes playerAttributes;
    int MaxHealth;
    public int currentHealth;
    bool dead;
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerAttributes = GetComponent<PlayerAttributes>();
        playerAttributes.OnPlayerHealthChange += UpdateHealth;
        currentHealth = playerAttributes.playerhealth;
        MaxHealth = currentHealth;
    }

    void Start()
    {
        dead = false;
    }
    void Update()
    {
        UpdateAim();
        CheckMeleeTimer();
        shootTimer += Time.deltaTime;


        if (Input.GetMouseButtonDown(0) && !IsPointerOnUI())
        {
            OnCloseCombat();
            UpdateMeleeAnimation();
            animator.SetTrigger("LeftMouse Trigger");
        }
        if (Input.GetMouseButtonDown(1) && !IsPointerOnUI())
        {
            OnRangedCombat();
        }
    }

    void UpdateAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        currentAimDirection = (mousePos - aim.position).normalized;

        // Melee rotation with +90° offset for visual alignment
        float meleeAngle = Mathf.Atan2(currentAimDirection.y, currentAimDirection.x) * Mathf.Rad2Deg + 90f;
        aim.rotation = Quaternion.Euler(0f, 0f, meleeAngle);
    }

    void OnCloseCombat()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
            audioSource.PlayOneShot(clips[0]);
        }
    }

    void UpdateHealth(int health)
    {
        currentHealth = health;
    }
    public void TakeDamage(bool hit, int damage)
    {
        if (hit)
        {
            int newHealth = currentHealth - damage;
            UpdateHealth(newHealth);
            Death();
        }
    }


    void Death()
    {
        if (currentHealth <= 0)
        {
            dead = true;
            animator.SetBool("Dead", dead);
            StartCoroutine(DestroyAfterAnimation());
           
        }
    }
   
    private IEnumerator DestroyAfterAnimation()
    {

        // Wait until the animator enters the death state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player death"))
        {
            yield return null;
        }

        // Get exact length of the death animation clip
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Debug.Log("player death animation:" + animationLength);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        

    }


    void OnRangedCombat()
    {
        if (shootTimer > shootCoolDown)
        {
            shootTimer = 0;

            //null check
            if (bullet == null)
            {
                Debug.LogError("Bullet prefab not assigned");
                return;
            }

            Debug.Log("Shooting direction: " + currentAimDirection);
            // Calculate bullet rotation WITHOUT the +90° offset
            float bulletAngle = Mathf.Atan2(currentAimDirection.y, currentAimDirection.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, bulletAngle);

            GameObject intBullet = Instantiate(bullet, aim.position, bulletRotation);
            intBullet.GetComponent<Rigidbody2D>().AddForce(currentAimDirection * firepower, ForceMode2D.Impulse);
            Destroy(intBullet, 2f);
            audioSource.PlayOneShot(clips[1]);
        }
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
                animator.ResetTrigger("LeftMouse Trigger"); //reset trigger
            }
        }
    }
    bool IsPointerOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void UpdateMeleeAnimation()
    {
        // Determine primary direction (prioritize larger axis)
        bool isHorizontalDominant = Mathf.Abs(currentAimDirection.x) > Mathf.Abs(currentAimDirection.y);

        if (isHorizontalDominant)
        {
            // Left/Right
            if (currentAimDirection.x < 0)
            {
                animator.SetInteger("Attack Direction", 3); // Left
            }
            else
            {
                animator.SetInteger("Attack Direction", 4); // Right
            }
        }
        else
        {
            // Up/Down
            if (currentAimDirection.y < 0)
            {
                animator.SetInteger("Attack Direction", 1); // up
            }
            else
            {
                animator.SetInteger("Attack Direction", 2); // dowm
            }
        }

    }   
    void OnDisable()
    {
        playerAttributes.OnPlayerHealthChange -= UpdateHealth;
    }
    
}
