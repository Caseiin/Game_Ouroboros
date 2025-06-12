using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float bounceHeight = 0.3f;
    public float bounceDuration = 0.4f;
    public int bounceCount = 2;

    public void StartBounce()
    {
        //call coroutine
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        float localHeight = bounceHeight;
        float LocalDuration = bounceDuration;

        for (int i = 0; i < bounceCount; i++)
        {
            //Another corotine to bounce
            yield return BounceRoutine(startPosition, localHeight, LocalDuration / 2);
            localHeight *= 0.5f;
            LocalDuration *= 0.8f;

        }

        transform.position = startPosition;
    }

    private IEnumerator BounceRoutine( Vector3 start, float height, float duration)
    {
        Vector3 peak = start + Vector3.up * height;
        float elapsed = 0f;

        // move upwards
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, peak, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        //Move downards
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(peak, start, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }
}
