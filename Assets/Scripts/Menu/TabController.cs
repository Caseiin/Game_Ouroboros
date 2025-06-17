
using UnityEngine;
using UnityEngine.UI;
public class Tab : MonoBehaviour
{
    public UnityEngine.UI.Image[] tabImages;
    public GameObject [] pages; // array of game objects

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActiveTabs(0);
    }

    public void ActiveTabs(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.gray;
        } //sets all tabs and pages to be unselected

        pages[tabNo].SetActive(true);
        //tabImages[tabNo].color = Color.white;

        switch (tabNo)
        {
            case 0:
                tabImages[tabNo].color = Color.red;
                break;
            case 1:
                tabImages[tabNo].color = new Color(92f, 64f, 51f);
                break;
            case 2:
                tabImages[tabNo].color = Color.yellow;
                break;
            case 3:
                tabImages[tabNo].color = Color.green;
                break;

        }
    }
}
