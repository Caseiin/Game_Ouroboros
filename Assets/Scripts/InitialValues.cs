using Unity.VisualScripting;
using UnityEngine;

public class InitialValues : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] PlayerAttributes player; //reference another script in a script
    [SerializeField] EnemyAttributes enemyAttributes;
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerAttributes not assigned in Inspector");
            return;
        }
        if (enemyAttributes == null)
        {
            Debug.LogError("EnemyAttributes not assigned in Inspector");
            return;            
        }

        player.playerhealth = 4;
        player.playermovespeed = 50f;
        player.playerheight = 4;
    }


}
