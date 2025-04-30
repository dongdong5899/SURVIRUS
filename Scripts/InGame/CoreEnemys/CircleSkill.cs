using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CircleSkill : PoolableMono
{
    public override void Init()
    {
        _sr.color = new Color(1, 0, 0, 0.5f);
        StartCoroutine("Pause");
    }

    private SpriteRenderer _sr;
    private CircleCollider2D _cc;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _cc = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        StartCoroutine("Pause");
    }

    void Update()
    {
            
    }

    IEnumerator Pause()
    {
        for (float i = 7; i > 0; i--)
        {
            _sr.DOFade(0.3f, i / 80);
            yield return new WaitForSeconds(i / 80);
            _sr.DOFade(0.5f, i / 80);
            yield return new WaitForSeconds(i / 80);
        }
        _sr.color = new Color(1, 0, 0, 0);
        transform.GetChild(0).gameObject.GetComponent<Light>().Lighting();
        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        CameraManager.Instance.CameraShake(0.6f, 12f);
        _cc.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _cc.enabled = false;
        Invoke("Destroy", 1.5f);
    }

    private void Destroy()
    {
        PoolManager.Instance.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControler>().HpDown(5f * DifficultyManager.Instance.Dm);
        }
    }
}
