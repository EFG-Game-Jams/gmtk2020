using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeterminismTest : MonoBehaviour
{
	public float minTime;
	public float variableTime;

	private void Start()
	{
		StartCoroutine(CoReload());
	}

	IEnumerator CoReload()
	{
		yield return new WaitForSeconds(minTime);
		yield return new WaitForSeconds(Random.Range(0, variableTime));
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
