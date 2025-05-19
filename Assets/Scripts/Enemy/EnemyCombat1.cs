using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class EnemyCombat1 : MonoBehaviour
{
    private EnemyAttributes enemyAttributes;
    private EnemyInitialValues enemyInitialValues;
    public GameObject ParticlePrefab;
    private GameObject Currentparticles;
    private Animator animator;
    [SerializeField] int enemyhealth;
    GameObject melee;

    bool isDead;
    int maxhealth;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAttributes = GetComponent<EnemyAttributes>();
        enemyAttributes.OnEnemyHealthChange += CurrentHealth;
        enemyhealth = enemyAttributes.EnemyHealth;
        Transform childTransform = transform.Find("Melee");
        melee = childTransform.gameObject;
    }

    void Start()
    {
        maxhealth = enemyhealth;
        isDead = false;
        melee.SetActive(false);
    }

    public void TakeDamage(bool hit,int damage)
    {
        if (hit)
        {
            int newHealth = enemyhealth - damage;
            CurrentHealth(newHealth);
            Death();
        }
    }

    void CurrentHealth(int health)
    {
        enemyhealth = health;
    }

    void Death()
    {
        if (enemyhealth <= 0 && !isDead)
        {
            isDead = true;
            animator.SetBool("Dead", isDead);
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            melee.SetActive(true);
        }
    }

    void OTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            melee.SetActive(false);
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {

        // Wait until the animator enters the death state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Snake dead"))
        {
            yield return null;
        }

        // Get exact length of the death animation clip
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(0.5f);
        Vector3 particleposition = transform.position;
        Currentparticles = Instantiate(ParticlePrefab, particleposition, Quaternion.identity);
        Destroy(gameObject);
       
    }

    void OnDisable()
    {
        enemyAttributes.OnEnemyHealthChange -= CurrentHealth;
    }
}
