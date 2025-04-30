using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : PoolableMono
{
    public override void Init()
    {
        
    }

    public void Set(float knockBack, float damege)
    {
        _knockBack = knockBack;
        _damege = damege;
    }

    public float _knockBack = 10;

    [SerializeField]
    private GameObject _blood;
    [SerializeField]
    private GameObject _sparcle;

    ParticleSystem ps;
    ParticleSystem.Particle[] p;

    public float _damege = 1;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        p = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
            ps.GetParticles(p); //가져오기
            Vector2 dir = (p[0].position - transform.position).normalized;
            other.GetComponent<EnemyControler>().HpDown(_damege, dir, _knockBack);

            PoolableMono hitParticle = PoolManager.Instance.Pop("HitParticle", p[0].position - p[0].velocity.normalized / 3);
            hitParticle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180));
        }

        if (other.tag == "Barrier")
        {
            ps.GetParticles(p); //가져오기
            Vector2 dir = (p[0].position - transform.position).normalized;

            PoolableMono sparcleParticle = PoolManager.Instance.Pop("SparcleParticle", p[0].position - p[0].velocity.normalized / 3);
            sparcleParticle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180));
        }
    }
}
