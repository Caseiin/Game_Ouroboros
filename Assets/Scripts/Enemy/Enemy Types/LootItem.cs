using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab;
    [Range(0, 100)] public float dropChance;
    
}

// Code reference:
// 1) Enemy drops Items on %:https://www.youtube.com/watch?v=Xe73unMxNiY