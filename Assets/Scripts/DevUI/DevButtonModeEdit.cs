using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevButtonModeEdit : MonoBehaviour
{
    public Button button;

    void Start()
    {
        if (SimulationState.Instance.CurrentMode != SimulationState.Mode.Simulate)
            button.interactable = false;

        button.onClick.AddListener(() => SimulationState.Instance.LoadScene(SimulationState.Mode.Edit));
    }
}
