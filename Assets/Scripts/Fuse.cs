using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public float timeToDetonate;

    void Start()
    {
        if (timeToDetonate > 0)
            StartCoroutine(CoDetonate());   
    }

    IEnumerator CoDetonate()
    {
        yield return new WaitForSeconds(timeToDetonate);
        GetComponent<Explosive>()?.Detonate();
        yield return null;
        Destroy(gameObject);
    }
}
