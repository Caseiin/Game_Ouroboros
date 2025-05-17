using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Combat : MonoBehaviour
{
    //melee variables
    public GameObject Melee;
    bool isAttacking = false;
    float atkDuration = 0.3f;
    float atkTimer = 0f;

    // ranged variables
    public Transform aim;
    public GameObject bullet;
    public float firepower = 10f;
    float shootCoolDown=0.25f;
    float shootTimer=0.5f;

    private bool IsPointerOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
        // checks if the mouse cursor is over a UI element
    }


    void Update()
    {
        CheckMeleeTimer();  
        shootTimer +=Time.deltaTime;
        if (Input.GetMouseButtonDown(0)&&(!IsPointerOnUI()))
        {
            //left mouse click triggers an attack from player
            OnCloseCombat();
        }
        if (Input.GetMouseButtonDown(1)&&(!IsPointerOnUI()))
        {
            //right click to shoot an enemy
            OnRangedCombat();
        }
    }

    void OnCloseCombat()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
            //call animator to play melee attack
        }
    }

    void CheckMeleeTimer ()
    {
        if (isAttacking)
        {
            atkTimer+= Time.deltaTime;
            if (atkTimer>=atkDuration)
            {
                atkTimer =0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }

    void OnRangedCombat()
    {
        if (shootTimer > shootCoolDown)
        {
            // problems: bullet is not moving also I cannot to understand the roll of aim
            shootTimer =0;
            GameObject intBullet = Instantiate(bullet,aim.position,aim.rotation); //creates new bullets
            intBullet.GetComponent<Rigidbody2D>().AddForce(-aim.up*firepower, ForceMode2D.Impulse);
            Destroy(intBullet,2f); //destroys the bullet after 2 seconds
        }
    }
    
}
