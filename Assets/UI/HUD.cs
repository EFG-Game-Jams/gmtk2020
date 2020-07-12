using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public Button buttonLevelSelect;
	public Button buttonReset;

	public TextMeshProUGUI textBombsDestroyed;
	public TextMeshProUGUI textFusesUsed;

	public ObjectiveInfoPanel objectiveBronze;
	public ObjectiveInfoPanel objectiveSilver;
	public ObjectiveInfoPanel objectiveGold;

	public void Start()
	{
		LevelDescriptor level = SimulationState.Instance.CurrentLevel;
		SimulationState.Mode mode = SimulationState.Instance.CurrentMode;

		if (level == null)
			return;

		buttonReset.interactable = (mode == SimulationState.Mode.Edit);

		objectiveBronze.Text = level.bombsForBronze.ToString();
		objectiveSilver.Text = level.bombsForSilver.ToString();
		objectiveGold.Text = level.bombsForGold.ToString();

		Refresh();
	}

	private void Update()
	{
		Refresh();
	}

	private void Refresh()
	{
		LevelDescriptor level = SimulationState.Instance.CurrentLevel;
		SimulationState.PlayState state = SimulationState.Instance.CurrentPlayState;
		if (level == null)
			return;

		textBombsDestroyed.text = state.destroyedBombs.ToString() + " / " + level.bombsInScene.ToString();
		textFusesUsed.text = state.fusedBombs.ToString() + " / " + level.bombsFuseable.ToString();

		objectiveBronze.Completed = (state.destroyedBombs >= level.bombsForBronze);
		objectiveSilver.Completed = (state.destroyedBombs >= level.bombsForSilver);
		objectiveGold.Completed = (state.destroyedBombs >= level.bombsForGold);
	}

	public void OnLevelSelectClicked()
	{
		SimulationState.Instance.ClearMode();
		SceneManager.LoadScene("LevelSelect");
	}
	public void OnResetClicked()
	{
		SimulationState.Instance.ResetEditing();
	}
}
