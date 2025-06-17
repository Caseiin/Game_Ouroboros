using UnityEngine;

public class Enemyweapon : MonoBehaviour
{
    public int weapondamage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager PlayerState = other.gameObject.GetComponent<PlayerStateManager>();

            // PlayerState.TakeHealth(weapondamage);

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
