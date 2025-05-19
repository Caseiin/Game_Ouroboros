using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject enemy;
    //public GameObject Archer;
    bool IsSpawned = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&IsSpawned==false)
        {
            //Instantiate(Archer,transform.position+ new Vector3(0,2,0),quaternion.identity); //enemy one
            Instantiate(enemy,transform.position+ new Vector3(0,-2,0),quaternion.identity); //enemy 2


            IsSpawned = true;
        }
    }
}
