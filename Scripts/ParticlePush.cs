using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePush : PoolableMono
{
    private ParticleSystem _ps;

    public override void Init()
    {

    }

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!_ps.isPlaying)
        {
            PoolManager.Instance.Push(this);
        }
    }
}
