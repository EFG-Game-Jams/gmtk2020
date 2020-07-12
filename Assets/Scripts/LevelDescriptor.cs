using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level Descriptor", menuName = "Game/Level Descriptor", order = 1)]
public class LevelDescriptor : ScriptableObject
{
	[Header("Auto - use context menu 'From active scene' option")]
	public int sceneIndex;
	public string sceneName;
	public int bombsInScene;

	[Header("Manual")]
	public string displayName;
	public int bombsFuseable;
	public int bombsForBronze;
	public int bombsForSilver;
	public int bombsForGold;

	[NonSerialized] public int highScore;

	[ContextMenu("From active scene")]
	public void FromActiveScene()
	{
		Scene scene = SceneManager.GetActiveScene();
		sceneIndex = scene.buildIndex;
		sceneName = scene.name;

		if (scene.buildIndex < 0)
			Debug.LogWarning("Scene '" + sceneName + "' is not included in build!");

		var roots = scene.GetRootGameObjects();
		foreach (var root in roots)
		{
			if (root.name != "Root")
				continue;

			for (int i = 0; i < root.transform.childCount; ++i)
			{
				Transform child = root.transform.GetChild(i);
				if (child.gameObject.name != "Bombs")
					continue;

				bombsInScene = child.childCount;
				break;
			}
		}
	}

	public void Load()
	{
		highScore = PlayerPrefs.GetInt(sceneName + "_highScore", 0);
	}
	public void Save()
	{
		PlayerPrefs.SetInt(sceneName + "_highScore", highScore);
		PlayerPrefs.Save();
	}

	[ContextMenu("Clear !ALL! PlayerPrefs for this project")]
	private void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
