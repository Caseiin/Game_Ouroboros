using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadQuit : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
        {
            Debug.LogError("Player not in inspector");
            return;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            SceneManager.LoadScene(2);
        }
        }
    }
