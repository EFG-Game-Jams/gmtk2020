using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectItem : MonoBehaviour
{
	public TextMeshProUGUI levelName;
	public ObjectiveInfoPanel objectiveBronze;
	public ObjectiveInfoPanel objectiveSilver;
	public ObjectiveInfoPanel objectiveGold;
	
	[SerializeField] private LevelDescriptor level;

	public bool IsValid => (level != null);
	public bool LevelPassed => (level.highScore >= level.bombsForBronze);
	
	public void SetLevel(LevelDescriptor level)
	{
		this.level = level;
		Refresh();
	}

	public void Refresh()
	{
		if (level == null)
		{
			gameObject.SetActive(false);
			return;
		}

		level.Load();

		levelName.text = level.displayName;

		objectiveBronze.Text = level.bombsForBronze.ToString();
		objectiveBronze.Completed = level.highScore >= level.bombsForBronze;

		objectiveSilver.Text = level.bombsForSilver.ToString();
		objectiveSilver.Completed = level.highScore >= level.bombsForSilver;

		objectiveGold.Text = level.bombsForGold.ToString();
		objectiveGold.Completed = level.highScore >= level.bombsForGold;
	}

	public void OnButtonClick()
	{
		SimulationState.Instance.LoadLevel(SimulationState.Mode.Edit, level);
	}
}
