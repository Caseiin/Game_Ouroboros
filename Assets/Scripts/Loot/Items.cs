
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int ID;
    public string Name;


    private HeartUIController heart;
    private BaseEnemy[]enemyInstances;
    private PlayerStateManager player;
    void Awake()
    {
        heart = FindFirstObjectByType<HeartUIController>();
        if (heart == null)
        {
            Debug.LogWarning("HeartUIController not found in the scene.");
        }

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemyInstances = new BaseEnemy[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemyInstances[i] = enemyObjects[i].GetComponent<BaseEnemy>();
        }
        player = FindFirstObjectByType<PlayerStateManager>();

    }

    public virtual void UseItem()
    {
        Debug.Log("Using item" + Name);
        // Have other items inherit from this class then use this function
        SoundEffectManager.Play("Skill Activation");

        switch (ID)
        {
            case 2:
                heart.Heal();
                break;
            case 3:
                foreach (BaseEnemy enemy in enemyInstances)
                {
                    if (enemy != null)
                    {
                        enemy.StartCoroutine(enemy.SpikeDamage());
                    }
                }
                break;
            case 4:
                player.StartSwordCoroutine(player.combatState.SwordRoutine());
                break;
            case 5:
                foreach (BaseEnemy enemy in enemyInstances)
                {
                    if (enemy != null)
                    {
                        enemy.StartCoroutine(enemy.TimeStop());
                    }
                }
                break;
            case 6:

                if (player.currentState == player.movingState)
                {
                    player.StartSpeedCoroutine(player.movingState.SpeedRoutine());
                }

                break;

        }

        

    }

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickUI.Instance != null)
        {
            ItemPickUI.Instance.ShowItemPickUp(Name, itemIcon);
            SoundEffectManager.Play("Gem");
        }
    }
}

//code references:
//1) https://www.youtube.com/watch?v=UV1OJ4Kg6wY&list=PLaaFfzxy_80HtVvBnpK_IjSC8_Y9AOhuP&index=12