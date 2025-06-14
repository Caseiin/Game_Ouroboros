using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour, IHealth
{
    Animator heartAnimator;              // Animator for heart damage/heal
    SpriteRenderer heartImage;                    // UI Image displaying heart sprite
    public Sprite[] heartStates;                // Different heart damage states (0 = full health)

    SpriteRenderer player;

    // Animation state names
    private readonly string[] hitStates = { "Damage1", "Damage2", "Damage3", "Damage4" };
    private readonly string[] healStates = { "Healing1", "Healing2", "Healing3" };

    private Dictionary<string, float> clipDurations;  // Holds durations of clips by name
    private int currentStateIndex = 0;                // Tracks current heart state

    void Awake()
    {
        heartAnimator = GetComponent<Animator>();
        heartImage = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

        // Load animation durations dynamically from Animator
        clipDurations = new Dictionary<string, float>();
        foreach (AnimationClip clip in heartAnimator.runtimeAnimatorController.animationClips)
        {
            if (!clipDurations.ContainsKey(clip.name))
            {
                clipDurations.Add(clip.name, clip.length);
            }
        }
    }

    // Triggered when player takes damage
    public void OnPlayerTakesDamage()
    {
        if (currentStateIndex >= heartStates.Length - 1)
        {
            Debug.Log("Heart already fully damaged");
            return;
        }

        string anim = hitStates[Mathf.Clamp(currentStateIndex, 0, hitStates.Length - 1)];
        float waitTime = clipDurations.ContainsKey(anim) ? clipDurations[anim] : 0.5f;

        heartAnimator.enabled = true;
        heartAnimator.Play(anim);

        //Flash red
        StartCoroutine(FlashRed());

        StartCoroutine(WaitAndSwitchToNextState(waitTime));
    }

    // Triggered when player receives healing
    public void TakeHealth()
    {
        if (currentStateIndex <= 0)
        {
            Debug.Log("Heart already at full health");
            return;
        }

        int healIndex = Mathf.Clamp(currentStateIndex - 1, 0, healStates.Length - 1);
        string anim = healStates[healIndex];
        float waitTime = clipDurations.ContainsKey(anim) ? clipDurations[anim] : 0.5f;

        heartAnimator.enabled = true;
        heartAnimator.Play(anim);


        StartCoroutine(WaitAndSwitchToPreviousState(waitTime));
    }

    // Fully resets heart
    public void GiveHealth()
    {
        currentStateIndex = 0;
        heartImage.sprite = heartStates[0];
        heartAnimator.enabled = false;
    }

    public void ResetHeart()
    {
        GiveHealth();
    }

    // Coroutine to switch to the next damage sprite
    private IEnumerator WaitAndSwitchToNextState(float delay)
    {
        yield return new WaitForSeconds(delay);
        heartAnimator.enabled = false;
        currentStateIndex++;
        heartImage.sprite = heartStates[currentStateIndex];
    }

    // Coroutine to switch to previous (healed) state
    private IEnumerator WaitAndSwitchToPreviousState(float delay)
    {
        yield return new WaitForSeconds(delay);
        heartAnimator.enabled = false;
        currentStateIndex--;
        heartImage.sprite = heartStates[currentStateIndex];
    }

    private IEnumerator FlashRed()
    {
        player.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        player.color = Color.white;
    }
}
