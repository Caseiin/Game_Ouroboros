using Microsoft.Unity.VisualStudio.Editor;
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
        for (int i =0; i< pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.red;
        } //sets all tabs and pages to be unselected

        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
    }
}
