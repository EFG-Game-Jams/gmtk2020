using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveInfoPanel : MonoBehaviour
{
	public Image image;
	public TextMeshProUGUI text;

	public Color[] medalColors;
	public int medal;
	
	public bool Completed
	{
		set
		{
			if (value)
				image.color = medalColors[medal];
			else
				image.color = medalColors[medal] * new Color(.5f, .5f, .5f, .25f);
		}
	}
	public string Text
	{ 
		set => text.text = value;
	}
}
