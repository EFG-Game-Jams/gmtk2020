using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public TextMeshPro timeDisplay;
    public float timeToDetonate;

    private bool lit;

    public bool IsLit => lit;

    void Start()
    {
        UpdateTimeDisplay();
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
            GetComponent<Bomb>()?.OnFuseLit();
            StartCoroutine(CoDetonate());
        }
    }

    IEnumerator CoDetonate()
    {
        var waitForPhysicsUpdate = new WaitForFixedUpdate();

        while (timeToDetonate > 0)
        {
            yield return waitForPhysicsUpdate;
            timeToDetonate -= Time.fixedDeltaTime;
            UpdateTimeDisplay();
        }

        GetComponent<Explosive>()?.Detonate();
        SimulationState.Instance.OnBombDestroyed();

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
