using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Knife : MonoBehaviour
{
    [SerializeField]
    private GameObject _swing;

    [SerializeField]
    private float _shootingDelay = 0.5f;
    private float _sd = 0f;

    [SerializeField]
    private float _damege = 4;
    [SerializeField]
    private float _knockBack = 7;

    [SerializeField]
    private GameObject _hitEffect;

    [SerializeField]
    private GameObject _swingControl;

    private PlayerControler _playerControler;

    private ParticleSystem _swingParticle;

    public bool _isReLoad = false;

    private void Awake()
    {
        _playerControler = GetComponentInParent<PlayerControler>();
        _swingParticle = _playerControler.transform.Find("Twirl").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_sd < _shootingDelay) _sd += Time.deltaTime;
        if (Input.GetMouseButton(0)) Swing();

        _swingControl.transform.position = transform.position;
    }

    private void Swing()
    {
        if (_sd >= _shootingDelay)
        {
            Vector3 dir = new Vector3(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
            RaycastHit2D[] hit2 = 
                Physics2D.BoxCastAll(transform.position + dir, 
                new Vector2(1.8f, 2f), 0, dir, 1.5f);

            List<RaycastHit2D> hits = new List<RaycastHit2D>();

            hits.AddRange(hit2);

            foreach (RaycastHit2D ray in hits)
            {
                if (ray.transform.CompareTag("Enemy"))
                {
                    Vector2 Edir = (ray.transform.position - transform.position).normalized;
                    ray.transform.GetComponent<EnemyControler>().HpDown(_damege, Edir, _knockBack);
                    PoolableMono swingParticle = PoolManager.Instance.Pop("HitParticle", ray.transform.position);
                    swingParticle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(Edir.y, Edir.x) * Mathf.Rad2Deg + 180));
                }
            }

            _swingParticle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z));
            _swingParticle.Play();
            _sd = 0f;
        }
    }
}