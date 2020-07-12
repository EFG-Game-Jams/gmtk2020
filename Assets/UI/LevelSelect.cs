using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Transform levelsGrid;

    void Start()
    {
        SimulationState.Instance.ClearMode();

        var items = GetItems();
        for (int i = 1; i < items.Count; ++i)
            items[i].GetComponent<Button>().interactable = items[i - 1].LevelPassed;
    }

    List<LevelSelectItem> GetItems()
    {
        List<LevelSelectItem> items = new List<LevelSelectItem>();
        for (int i=0; i<levelsGrid.childCount; ++i)
        {
            var item = levelsGrid.GetChild(i).GetComponent<LevelSelectItem>();
            item.Refresh();
            if (item != null && item.IsValid)
                items.Add(item);
        }
        return items;
    }
}
