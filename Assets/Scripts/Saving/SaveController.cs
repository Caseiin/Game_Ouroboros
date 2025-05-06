using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    string saveLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //define the save location
        saveLocation = Path.Combine(Application.persistentDataPath,"saveData.json"); //fire directory of the saveData json file
        Debug.Log("save location: "+saveLocation); //path location for debugging and other purposes
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
            playerposition = GameObject.FindGameObjectWithTag("Player").transform.position 
        };

        //write data to textfile
        File.WriteAllText(saveLocation,JsonUtility.ToJson(saveData)); //File directory set new Data to the a json SaveData
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            // reverse of save is done to load from save file to game
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerposition;
        }
        else
        {
            SaveGame(); //ensures as game starts up you have an initial save point
        }
    }
}
