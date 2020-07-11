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
        if (timeToDetonate > 0)
            LightFuse(timeToDetonate); 
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

            int displayTime = Mathf.CeilToInt(timeToDetonate);
            timeDisplay.text = displayTime.ToString();
        }

        GetComponent<Explosive>()?.Detonate();
        yield return null;
        Destroy(gameObject);
    }
}
