using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private static bool loadFromDoor;

    //Player requirements
    private GameObject _player;
    private Collider2D _playercoll;
    private Collider2D _doorColl;
    private Vector3 _playerSpawnPos;

    private DoorsInteraction.DoorSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _player = GameObject.FindGameObjectWithTag("Player");
        _playercoll = _player.GetComponent<Collider2D>();

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
        SceneFadeManager.instance.StartFadeOut();

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
            FindDoor(_doorToSpawnTo);
            _player.transform.position = _playerSpawnPos;
            loadFromDoor = false;
        }
    }

    private void FindDoor(DoorsInteraction.DoorSpawnAt doorSpawnNumber)
    {
        DoorsInteraction[] doors = FindObjectsByType<DoorsInteraction>(FindObjectsSortMode.None);

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].currentDoorPosition == doorSpawnNumber)
            {
                _doorColl = doors[i].gameObject.GetComponent<Collider2D>();
                CalculateSpawnPosition();
                return;
            }
        }
    }

    private void CalculateSpawnPosition()
    {
        float ColliderHeight = _playercoll.bounds.extents.y;
        _playerSpawnPos = _doorColl.transform.position - new Vector3(0f, ColliderHeight, 0f);
    }
}
