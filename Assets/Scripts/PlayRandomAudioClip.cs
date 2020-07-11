using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomAudioClip : MonoBehaviour
{
	public AudioClip[] clips;
    public float randomDelay;

	private void Start()
	{
        var audioSource = GetComponent<AudioSource>();

        int index = Mathf.FloorToInt(Random.Range(0, clips.Length - .001f));
        audioSource.clip = clips[index];
        audioSource.PlayDelayed(Random.Range(0, randomDelay));
    }
}
