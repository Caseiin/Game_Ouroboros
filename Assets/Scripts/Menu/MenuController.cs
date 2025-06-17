using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    void Start()
    {
        menuCanvas.SetActive(false); //during start up menu object is disabled
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menuCanvas.activeSelf && PauseController.isGamePaused) return;

            bool isActive = !menuCanvas.activeSelf;
            menuCanvas.SetActive(isActive); // toggles between menu off and on

            PauseController.SetPause(menuCanvas.activeSelf);
           
        }
    }
}
