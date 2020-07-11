using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevButtonModeSimulate : MonoBehaviour
{
    public Button button;

    void Start()
    {
        if (SimulationState.Instance.CurrentMode != SimulationState.Mode.Edit)
            button.interactable = false;

        button.onClick.AddListener(() => SimulationState.Instance.LoadLevel(SimulationState.Mode.Simulate));
    }
}
