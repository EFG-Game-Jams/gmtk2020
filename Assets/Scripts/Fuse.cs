using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public const float FuseTicksPerSecond = 4;

    public TextMeshPro timeDisplay;
    public float timeToDetonate;
    public bool forbidPlayerInteraction;

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
            return;
            //timeToDetonate = Mathf.Min(timeToDetonate, seconds);
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
        GetComponent<Bomb>()?.OnDetonate();

        yield return null;
        Destroy(gameObject);
    }

    public void UpdateTimeDisplay()
    {
        if (timeDisplay == null)
            return;

        int displayTime = Mathf.CeilToInt(timeToDetonate * FuseTicksPerSecond);
        if (displayTime == 0)
            timeDisplay.text = "";
        else
            timeDisplay.text = displayTime.ToString();
    }

    private void OnMouseDown()
    {
        if (forbidPlayerInteraction)
            return;

        if (Input.GetMouseButtonDown(0))
            FuseInteractionHandler.Instance.BeginInteraction(this);
    }
}
