using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    InventoryController inventoryController;
    HotBarController hotBarController;

    Chest[] chests;
    string saveLocation;
    string initialGameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //define the save location
        InitialiseComponenets();
        Debug.Log("save location: " + saveLocation); //path location for debugging and other purposes
        Debug.Log("initialData location: " + initialGameData); //path location for debugging and other purposes

        LoadGame();
    }

    void InitialiseComponenets()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json"); //fire directory of the saveData json file
        initialGameData = Path.Combine(Application.persistentDataPath, "initialData.json");
        inventoryController = FindAnyObjectByType<InventoryController>();
        hotBarController = FindAnyObjectByType<HotBarController>();
        chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
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
            inventorySaveData = inventoryController.GetInvItem(),
            hotBarSaveData = hotBarController.GetHotbarItem(),
            chestSaveData = GetChestsState()
        };

        //write data to textfile
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData)); //File directory set new Data to the a json SaveData
        Debug.Log("game was saved");
    }

    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestState = new List<ChestSaveData>();

        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData()
            {
                ChestID = chest.ChestID,
                isOpened = chest.IsOpened
            };
            chestState.Add(chestSaveData);
        }

        return chestState;
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
            hotBarController.SetHotbarItem(saveData.hotBarSaveData);

            //LoadChestState
            LoadChestStates(saveData.chestSaveData);
        }
        else
        {
            SaveGame();
            inventoryController.SetInvItem( new List<InventorySaveData>());
            hotBarController.SetHotbarItem(new List<InventorySaveData>());
        }
    }

    public void LoadInitial()
    {
        //Loads initial file 
    }

    void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.ChestID == chest.ChestID);

            if (chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }
}
