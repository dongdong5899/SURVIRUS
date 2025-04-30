using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCore : MonoBehaviour
{
    private float _defaltSkillDelay = 0;

    private float _DSD = 1f;
    private bool _onSkill = true;

    [SerializeField]
    private GameObject _circleAttack;

    private GameObject _player;
    private PlayerControler _pc;
    private AudioSource _as;

    [SerializeField]
    private AudioClip[] _ac;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _player = GameManager.Instance._player;
        _pc = _player.GetComponent<PlayerControler>();
    }

    private void Update()
    {
        if (_defaltSkillDelay > 0)
        {
            _defaltSkillDelay -= Time.deltaTime;
            if (_defaltSkillDelay < 0)
                _defaltSkillDelay = 0;
        }

        if (GetComponent<EnemyControler>().Hp < 0.1f)
        {
            _onSkill = false;
            if (_pc._coreDirOn[1]) _pc._coreDirOn[1] = false;
        }

        Vector3 dir = _player.transform.position - transform.position;
        if (Mathf.Abs((dir).magnitude) < 40f && _onSkill)
        {
            Skill();
            if (!_pc._coreDirOn[1]) _pc._coreDirOn[1] = true;
        }
        else
            if (_pc._coreDirOn[1]) _pc._coreDirOn[1] = false;
    }

    public void OnEventSkill()
    {
        _DSD = 0.5f;
        StartCoroutine(SpawnSkillControl());
    }

    public void Skill()
    {
        if (_defaltSkillDelay == 0)
        {
            _defaltSkillDelay = _DSD;
            Invoke("BoomSound", 0.7f);
            StartCoroutine("BeepSound");
            DefaltAttack();
        }
    }

    private void BoomSound()
    {
        _as.PlayOneShot(_ac[0]);
    }

    IEnumerator BeepSound()
    {
        for (float i = 7; i > 0; i--)
        {
            _as.PlayOneShot(_ac[1]);
            yield return new WaitForSeconds(i / 40);
        }
    }

    private void DefaltAttack()
    {
        for (int i = 0; i < 20; i ++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-30f, 30f), Random.Range(-17f, 17f));
            PoolManager.Instance.Pop("Circle", _player.transform.position + randomPos);
        }
    }

    IEnumerator SpawnSkillControl()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnSkill();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnSkill()
    {
        float r = 0;
        Vector2 dir;
        for (int i = 0; i < 5; i++)
        {
            r += 24;
            dir = new Vector2(Mathf.Cos(r), Mathf.Sin(r)) * 10;
            PoolManager.Instance.Pop("Enemy", transform.position, dir, 0);
        }
    }
}
