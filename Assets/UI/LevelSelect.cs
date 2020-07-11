using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    void Start()
    {
        SimulationState.Instance.ClearMode();
                
        var items = GetComponentsInChildren<LevelSelectItem>();
        foreach (var item in items)
            item.Refresh();
    }
}
