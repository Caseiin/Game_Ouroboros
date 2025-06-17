using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsInteraction : MonoBehaviour, IInteractable
{
    public enum DoorSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four
    }

    [Header("Spawn To")]
    [SerializeField] private SceneField _sceneToLoad;
    [SerializeField] private DoorSpawnAt DoorToSpawnTo;


    [Space(10f)]
    [Header("This Door")]
    public DoorSpawnAt currentDoorPosition;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {

        if (CanInteract())
        {
            SceneController.SwapSCeneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
        }
       
    }
}
