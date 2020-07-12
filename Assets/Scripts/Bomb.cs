using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Audio
    private AudioSource audioSource;
    public AudioClip[] clipsOnSelect;
    public AudioClip[] clipsOnDamage;
    public AudioClip[] clipsOnNoFuse;

    // Eyes
    public SpriteRenderer eyesSprite;
    public SpriteRenderer pupilsSprite;
    public Sprite[] eyesVariants;
    private Vector3 pupilsOrigin;
    public int eyesIndex = -1;
    private float nextBlink;
    private bool blinking;

    // Fuse
    public GameObject fuseSparkleEffect;

    private void OnValidate()
    {
        //if (eyesIndex < 0)
            eyesIndex = Mathf.FloorToInt(Random.Range(0, eyesVariants.Length - .001f));
    }

    void Awake()
    {
        if (IsAlive)
        {
            audioSource = GetComponent<AudioSource>();

            eyesSprite.sprite = eyesVariants[eyesIndex];

            pupilsOrigin = pupilsSprite.transform.localPosition;
		    nextBlink = Random.Range(2, 10);
        }
        else
        {
            eyesSprite.enabled = false;
            pupilsSprite.enabled = false;
        }
    }

    private void Update()
    {
        UpdateEyes();
    }

    private void UpdateEyes()
    {
        if (!IsAlive)
            return;

        Vector3 mousePx = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePx);
        mouseWorld.z = 0;

        Vector3 diff = mouseWorld - transform.position;
        Vector3 dir = diff.normalized;

        Vector3 offset = dir * .015f;
        pupilsSprite.transform.localPosition = pupilsOrigin + offset;

        nextBlink -= Time.deltaTime;
        if (nextBlink <= 0)
        {
            if (blinking)
            {
                pupilsSprite.transform.localScale = Vector3.one;
                nextBlink = Random.Range(2, 6);
            }
            else
            {
                pupilsSprite.transform.localScale = new Vector3(1, .3f, 1);
                nextBlink = Random.Range(.15f, .3f);
            }
            blinking = !blinking;
        }
    }

    public void OnFuseLit()
    {
        fuseSparkleEffect.SetActive(true);
    }
    public void OnDetonate()
    {
        SimulationState.Instance.OnBombDestroyed();
    }

    public void PlaySelectFx()
    {
        if (IsAlive)
            PlayAudioClipRandom(clipsOnSelect);
    }
    public void PlayNoFuseFx()
    {
        if (IsAlive)
            PlayAudioClipRandom(clipsOnNoFuse);
    }
    public void PlayDamageFx()
    {
        if (IsAlive)
            PlayAudioClipRandom(clipsOnDamage, Random.Range(.2f, .6f));
    }

    private void PlayAudioClipRandom(AudioClip[] clips, float delay = 0f)
    {
        int index = Mathf.FloorToInt(Random.Range(0, clips.Length - .001f));
        PlayAudioClip(clips[index], delay);
    }

    private void PlayAudioClip(AudioClip clip, float delay)
    {
        if (audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.PlayDelayed(delay);
    }

    private bool IsAlive
    {
        get
        {
            var fuse = GetComponent<Fuse>();
            return fuse != null && !fuse.forbidPlayerInteraction;
        }
    }
}
