using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Light : PoolableMono
{
    public override void Init()
    {

    }

    Tween t;

    private void Awake()
    {

    }

    public void Lighting()
    {
        if (t != null && t.IsActive())
            t.Kill();
        GetComponent<Light2D>().intensity = 4;
        t = DOTween.To(() =>
            GetComponent<Light2D>().intensity,
            value => GetComponent<Light2D>().intensity = value,
            0, 0.15f);
    }
}
