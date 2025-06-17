using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTracker : MonoBehaviour
{
    [SerializeField] private Transform mapRoomContainer;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        UpdateMapRoomForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMapRoomForScene(scene.name);
    }

    void UpdateMapRoomForScene(string sceneName)
    {
        if (mapRoomContainer == null)
        {
            Debug.LogError("MapRoomContainer reference is missing!");
            return;
        }

        foreach (Transform child in mapRoomContainer)
        {
            bool match = child.name == sceneName;
            child.gameObject.SetActive(match);
        }

        Debug.Log($"[MapTracker] Activated map for scene: {sceneName}");
    }
}
