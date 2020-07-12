using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : TransientSingleton<MusicPlayer>
{
	private void Awake()
	{
		var all = FindObjectsOfType<MusicPlayer>();
		if (all.Length > 1)
			Destroy(gameObject);
		else
			DontDestroyOnLoad(gameObject);
	}
}
