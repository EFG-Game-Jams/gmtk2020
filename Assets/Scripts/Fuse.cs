using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public TextMeshPro timeDisplay;
    public float timeToDetonate;

    private bool lit;

    void Start()
    {
        timeDisplay.text = "";
    }
    
    public void StartSimulation()
    {
        if (timeToDetonate > 0)
            LightFuse(timeToDetonate);
    }

    public void SetTimeToDetonate(float newTime)
    {
        timeToDetonate = newTime;
        UpdateTimeDisplay();
    }

    public void LightFuse(float seconds)
    {
        if (lit)
        {
            timeToDetonate = Mathf.Min(timeToDetonate, seconds);
        }
        else
        {
            timeToDetonate = seconds;
            lit = true;
            StartCoroutine(CoDetonate());
        }
    }

    IEnumerator CoDetonate()
    {
        var waitForPhysicsUpdate = new WaitForFixedUpdate();

        while (timeToDetonate > 0)
        {
            yield return waitForPhysicsUpdate;
            timeToDetonate -= Time.deltaTime;
            UpdateTimeDisplay();
        }

        GetComponent<Explosive>()?.Detonate();
        yield return null;
        Destroy(gameObject);
    }

    public void UpdateTimeDisplay()
    {
        int displayTime = Mathf.CeilToInt(timeToDetonate);
        if (displayTime == 0)
            timeDisplay.text = "";
        else
            timeDisplay.text = displayTime.ToString();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
            FuseInteractionHandler.Instance.BeginInteraction(this);
    }
}
