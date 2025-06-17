using UnityEngine;

public class Trapweapon : MonoBehaviour
{
    public int weapondamage = 1;
    HeartUIController heart;


    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager PlayerState = other.gameObject.GetComponent<PlayerStateManager>();

            heart = FindFirstObjectByType<HeartUIController>();
            if (heart == null)
            {
                Debug.LogWarning("HeartUIController not found in the scene.");
            }

            heart.TakeHealth(1);

            if (gameObject.name == "melee")
            {
                SoundEffectManager.Play("Meleehit");
            }
            else
            {
                SoundEffectManager.Play("Arrowhit");
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyStateManager enemyState = other.gameObject.GetComponent<EnemyStateManager>();

            enemyState.TakeHealth(weapondamage);

            if (gameObject.name == "melee")
            {
                SoundEffectManager.Play("Meleehit");
            }
            else
            {
                SoundEffectManager.Play("Arrowhit");
                Destroy(gameObject);
            }
        }


    }
}
