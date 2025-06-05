using UnityEngine;
using System.Collections;
public class Attack_Range : MonoBehaviour
{
    public GameObject melee;
    public float attackCooldown = 2f;  // Time between attacks
    public float attackDuration = 0.5f;  // How long attack stays active
    
    public bool isAttacking = false;
    private bool playerInRange = false;
    private Animator animator;

    void Start()
    {
        if (melee == null)
        {
            Debug.LogError("Enemy melee object not assigned in inspector");
            return;
        }

        melee.SetActive(false);
        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.Log("Animator from parent not connected to child");
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isAttacking)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            melee.SetActive(false);
        }
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        while (playerInRange)
        {
            // Start attack
            melee.SetActive(true);
            
            // Trigger attack animation if exists
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            // Keep attack active for duration
            yield return new WaitForSeconds(attackDuration);
            
            // Deactivate attack
            melee.SetActive(false);

            // Cooldown before next attack
            yield return new WaitForSeconds(attackCooldown);
        }
        
        isAttacking = false;
    }
}