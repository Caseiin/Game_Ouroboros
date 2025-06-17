using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //this allows us to pack and unpack data
public class SaveData
{
   // This script must not be monobehaviour
   public Vector3 playerposition;
   public string sceneName;
   public List<InventorySaveData> inventorySaveData;
   public List<InventorySaveData> hotBarSaveData;

   public List<ChestSaveData> chestSaveData;
   
}

[System.Serializable]

public class ChestSaveData
{
   public string ChestID;
   public bool isOpened;
}