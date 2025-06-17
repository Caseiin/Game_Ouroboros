using UnityEngine;

public class Enemyweapon : MonoBehaviour
{
    public int weapondamage = 1;
    HeartUIController heart;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

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

        
    }
}
