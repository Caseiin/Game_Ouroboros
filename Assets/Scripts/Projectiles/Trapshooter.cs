using System.Collections;
using UnityEngine;

public class Trapshooter : MonoBehaviour
{
    public GameObject projectile;
    public float shootInterval = 0.9f; // Time between shots

    void Start()
    {
        if (projectile == null)
        {
            Debug.Log("Projectile not connected in inspector");
            return;
        }

        StartCoroutine(TrapShoot()); // Start once
    }

    private IEnumerator TrapShoot()
    {
        while (true)
        {
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();


            rb.AddForce(Vector3.down * 10f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(shootInterval); // Wait before shooting next

            Destroy(bullet, 1f);
        }
    }
}
