using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    void Start()
    {
        menuCanvas.SetActive(false); //during start up menu object is disabled
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = !menuCanvas.activeSelf;
            menuCanvas.SetActive(isActive); // toggles between menu off and on
            Time.timeScale = menuCanvas.activeSelf ? 0f : 1f; //check this code           
        }
    }
}
