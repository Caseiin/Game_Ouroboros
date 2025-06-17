using Mono.Cecil.Cil;
using UnityEngine;

public static class GlobalHelper
{
    public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}"; //Chest_3_4
    }
    
}

// Code reference
// 1) The idea of static classes for generic tasks was inspired by this code: https://www.youtube.com/watch?v=MPP9GLp44Pc