using UnityEngine;
using UnityEngine.EventSystems;

public class Combat : MonoBehaviour
{
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
    float shootTimer = 0.5f;
    
    // Added direction storage
    private Vector2 currentAimDirection;


    void Update()
    {
        UpdateAim();
        CheckMeleeTimer();  
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && !IsPointerOnUI())
        {
            OnCloseCombat();
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

        // // Optional melee flip
        // float baseSize = 0.2f; // Your desired size multiplier
        // Melee.transform.localScale = currentAimDirection.x < 0 
        // ? new Vector3(-baseSize, baseSize, 1f) 
        // : new Vector3(baseSize, baseSize, 1f);
    }

    void OnCloseCombat()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
        }
    }

    void OnRangedCombat()
    {
        if (shootTimer > shootCoolDown)
        {
            shootTimer = 0;
            
            // Calculate bullet rotation WITHOUT the +90° offset
            float bulletAngle = Mathf.Atan2(currentAimDirection.y, currentAimDirection.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, bulletAngle);
            
            GameObject intBullet = Instantiate(bullet, aim.position, bulletRotation);
            intBullet.GetComponent<Rigidbody2D>().AddForce(currentAimDirection * firepower, ForceMode2D.Impulse);
            Destroy(intBullet, 2f);
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
            }
        }
    }
    bool IsPointerOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
