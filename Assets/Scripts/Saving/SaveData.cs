using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //this allows us to pack and unpack data
public class SaveData 
{
   // This script must not be monobehaviour
   public Vector3 playerposition;
   public List<InventorySaveData> inventorySaveData;
}
