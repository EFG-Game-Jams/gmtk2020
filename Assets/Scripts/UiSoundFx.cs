﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSoundFx : MonoBehaviour
{
	public AudioSource source;
	public AudioClip clipAdjustFuse;

	public void PlayAdjustFuse() => Play(clipAdjustFuse);

	private void Play(AudioClip clip)
	{
		/*if (source.isPlaying)
			return;*/
		source.clip = clip;
		source.Play();
	}

	private static UiSoundFx instance;
	public static UiSoundFx GetOrCreate()
	{
		if (instance == null)
		{
			UiSoundFx prefab = Resources.Load<UiSoundFx>("UiSoundFx");
			instance = Instantiate(prefab);
		}
		return instance;
	}
}
