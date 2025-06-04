using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    InventoryController inventoryController;
    string saveLocation;
    string initialGameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //define the save location
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json"); //fire directory of the saveData json file
        initialGameData = Path.Combine(Application.persistentDataPath, "initialData.json");
        inventoryController = FindAnyObjectByType<InventoryController>();
        Debug.Log("save location: " + saveLocation); //path location for debugging and other purposes
        Debug.Log("initialData location: " + initialGameData); //path location for debugging and other purposes
        LoadGame();
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("Player not found");
            return;
        }

        SaveData saveData = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            playerposition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventoryController.GetInvItem()
        };

        //write data to textfile
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData)); //File directory set new Data to the a json SaveData
        Debug.Log("game was saved");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            string currentScene = SceneManager.GetActiveScene().name;

            // Only load position if in the same scene
            if (saveData.sceneName == currentScene)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = saveData.playerposition;
                }
            }
            else
            {
                Debug.LogWarning("Saved position is from different scene. Using default position.");
            }

            inventoryController.SetInvItem(saveData.inventorySaveData);
        }
        else
        {
            SaveGame();
        }
    }

    public void LoadInitial()
    {
        //Loads initial file 
    }
}
