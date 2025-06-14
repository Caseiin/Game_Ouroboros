using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private static bool loadFromDoor;
    private DoorsInteraction.DoorSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoded;
    }
    public static void SwapSCeneFromDoorUse(SceneField myScene, DoorsInteraction.DoorSpawnAt doorToSpawnAt)
    {
        loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorsInteraction.DoorSpawnAt doorSpawnAt = DoorsInteraction.DoorSpawnAt.None)
    {
        SceneFadeManager.instance.StarFadeOut();

        while (SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }

        _doorToSpawnTo = doorSpawnAt;
        SceneManager.LoadScene(myScene);
    }

    //called when a new scene is loaded (included start of the game)

    private void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();

        if (loadFromDoor)
        {
            //warp player to correct location based on door


            loadFromDoor = false;
        }
    }
}
