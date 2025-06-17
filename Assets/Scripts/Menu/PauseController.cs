using Mono.Cecil.Cil;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool isGamePaused { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        isGamePaused = pause;
    }
}

// Code references:
// 1) The use of static functions and variables was inspired by this: https://www.youtube.com/watch?v=fspxIduosYQ