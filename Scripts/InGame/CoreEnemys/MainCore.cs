using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCore : MonoBehaviour
{
    [SerializeField]
    private GameObject _shield;
    private GameObject _bMovingRockImage;
    private GameObject _bChainImage;
    private SpriteRenderer _bMovingRockSR;
    private BoxCollider2D _bc2d;
    private GameObject _player;

    private PlayerControler _pc;

    private float _ws = 0f;
    private float _rbs = 0f;
    private float _rbsDelay = 1f;
    private int _spawnCount = 4;
    private bool _onEventSkill = false;

    private void Awake()
    {
        _bMovingRockImage = transform.GetChild(0).gameObject;
        _bChainImage = transform.GetChild(1).gameObject;
        _bMovingRockSR = _bMovingRockImage.GetComponent<SpriteRenderer>();
        _bc2d = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _player = GameManager.Instance._player;
        _pc = _player.GetComponent<PlayerControler>();
        Invoke("OnColl", 2);
    }

    void Update()
    {
        if (GameManager.Instance._dieCoreCount == 4)
        {
            _shield.SetActive(false);
            _pc._coreDirOn[0] = true;
        }
        if (!_shield.activeSelf)
        {
            _bMovingRockImage.transform.position = _player.transform.position;

            if (_ws < 4f) _ws += Time.deltaTime;

            _rbs -= Time.deltaTime;
            if (_rbs <= 0f)
            {
                _rbs = _rbsDelay;
                int r = Random.Range(0, 4);
                if (r != 0)
                {
                    RedSkill();
                }
                else
                {
                    _rbs += 2f;
                    BlueSkill();
                }
            }
        }
    }

    public void OnEventSkill()
    {
        _rbsDelay = 0.8f;
        _spawnCount = 8;
        _onEventSkill = true;
    }

    private void OnColl()
    {
        _bc2d.enabled = true;
    }

    public void WhiteSkill()
    {
        if (_ws >= 4f)
        {
            _ws = 0f;
            StartCoroutine("Spawn", _spawnCount);
        }
    }

    private void RedSkill()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-30f, 30f), Random.Range(-17f, 17f));
            PoolManager.Instance.Pop("Circle", _player.transform.position + randomPos);
        }
    }
    private void BlueSkill()
    {
        _bMovingRockImage.SetActive(true);
        StartCoroutine("Pause");
    }

    IEnumerator Spawn(int count)
    {
        Vector2 force;
        for (int i = 0; i < count; i++)
        {
            force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10;
            PoolManager.Instance.Pop("Enemy", transform.position, force, 0);
            yield return new WaitForSeconds(0.5f / count);
        }
        if (_onEventSkill)
        {
            force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10;
            PoolManager.Instance.Pop("EliteEnemy", transform.position, force, 0);
        }
    }

    IEnumerator Pause()
    {
        for (float i = 7; i > 0; i--)
        {
            _bMovingRockSR.DOFade(0.5f, i / 80);
            yield return new WaitForSeconds(i / 80);
            _bMovingRockSR.DOFade(0.8f, i / 80);
            yield return new WaitForSeconds(i / 80);
        }
        _bMovingRockSR.color = new Color(1, 0, 0, 0);
        _bChainImage.transform.position = _player.transform.position;
        _bChainImage.SetActive(true);
        GameManager.Instance._moving = false;
        Invoke("MovingUnrock", 2f);
        _bMovingRockImage.SetActive(false);
    }

    private void MovingUnrock()
    {
        _bChainImage.SetActive(false);
        GameManager.Instance._moving = true;
    }
}
