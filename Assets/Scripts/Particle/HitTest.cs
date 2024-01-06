using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTest : MonoBehaviour
{
    public ParticleSystem HitPar;
    float TimeExsisted = 0;
    void Awake()
    {
        HitPar.Play();
    }

    void Update()
    {
        TimeExsisted += Time.deltaTime;
        if (TimeExsisted >= 1)
            Destroy(gameObject);
    }

}
