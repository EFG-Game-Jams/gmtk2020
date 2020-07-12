using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulationCompletePanel : MonoBehaviour
{
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI highScoreText;
	public Button retryButton;

	private static SimulationCompletePanel instance;

	private void Awake()
	{
		instance = this;
		gameObject.SetActive(false);
	}

	public static void Activate()
	{
		if (instance == null)
			throw new System.Exception("No instance of SimulationCompletePanel in scene!");

		int score = SimulationState.Instance.CurrentPlayState.destroyedBombs;
		int passScore = SimulationState.Instance.CurrentLevel.bombsForBronze;
		int highScore = SimulationState.Instance.CurrentLevel.highScore;
		int maxScore = SimulationState.Instance.CurrentLevel.bombsInScene;

		instance.titleText.text = (score >= passScore) ? "Level Completed" : "Level Failed";
		instance.scoreText.text = score.ToString() + " / " + maxScore.ToString();
		instance.highScoreText.text = highScore.ToString() + " / " + maxScore.ToString();

		instance.gameObject.SetActive(true);
	}

	public void OnRetryClicked()
	{
		SimulationState.Instance.LoadLevel(SimulationState.Mode.Edit);
	}
	public void OnLevelSelectClicked()
	{
		SimulationState.Instance.ClearMode();
		SceneManager.LoadScene("LevelSelect");
	}
}
