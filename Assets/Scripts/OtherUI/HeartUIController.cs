using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour, IHealth
{
    Animator heartAnimator;
    SpriteRenderer heartImage;
    public Sprite[] heartStates;

    SpriteRenderer player;

    private readonly string[] hitStates = { "Damage1", "Damage2", "Damage3", "Damage4" };
    private readonly string[] healStates = { "Healing1", "Healing2", "Healing3" };

    private Dictionary<string, float> clipDurations;
    private int currentStateIndex = 0;
    PlayerStateManager playerState;

    void Awake()
    {
        heartAnimator = GetComponent<Animator>();
        heartImage = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateManager>();

        clipDurations = new Dictionary<string, float>();
        foreach (AnimationClip clip in heartAnimator.runtimeAnimatorController.animationClips)
        {
            clipDurations.TryAdd(clip.name, clip.length);
        }
    }

    /// <summary>
    /// Damage the player, increase damage state
    /// </summary>
    public void TakeHealth(int damage)
    {
        if (currentStateIndex >= heartStates.Length - 1)
        {
            Debug.Log("Heart already fully damaged");
            StartCoroutine(playerState.DeathRoutine());
            return;
        }

        string anim = hitStates[Mathf.Clamp(currentStateIndex, 0, hitStates.Length - 1)];
        float waitTime = clipDurations.ContainsKey(anim) ? clipDurations[anim] : 0.5f;

        heartAnimator.enabled = true;
        heartAnimator.Play(anim);
        StartCoroutine(FlashRed());
        StartCoroutine(WaitAndSwitchToNextState(waitTime));
    }

    /// <summary>
    /// Heal the player, reduce damage state
    /// </summary>
    public void Heal()
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

    /// <summary>
    /// Fully restores the heart (for example on respawn or power-up)
    /// </summary>
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

    private IEnumerator WaitAndSwitchToNextState(float delay)
    {
        yield return new WaitForSeconds(delay);
        heartAnimator.enabled = false;
        currentStateIndex++;
        heartImage.sprite = heartStates[currentStateIndex];
    }

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
