using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlueCore : MonoBehaviour
{
    private GameObject _movingRockImage;
    private GameObject _ChainImage;
    private SpriteRenderer _movingRockSR;
    private EnemyControler _ec;

    private float _rockDelay = 0;

    private GameObject _player;
    private PlayerControler _pc;
    private AudioSource _as;
    
    [SerializeField]
    private AudioClip _ac;
    [SerializeField] GameObject _barrier;

    private void Awake()
    {
        _movingRockImage = transform.GetChild(0).gameObject;
        _ChainImage = transform.GetChild(1).gameObject;
        _movingRockSR = _movingRockImage.GetComponent<SpriteRenderer>();
        _ec = GetComponent<EnemyControler>();
        _as = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _player = GameManager.Instance._player;
        _pc = _player.GetComponent<PlayerControler>();
        _barrier.transform.position = transform.position;
    }

    private void Update()
    {
        _movingRockImage.transform.position = _player.transform.position;


        if (_ec.Hp < 0.1f)
        {
            _barrier.SetActive(false);
            _movingRockImage.SetActive(false);
            if (_pc._coreDirOn[3]) _pc._coreDirOn[3] = false;
            StopAllCoroutines();
        }
        else
        {
            Vector3 dir = _player.transform.position - transform.position;
            if (Mathf.Abs((dir).magnitude) < 40f)
            {
                if (!_pc._coreDirOn[3]) _pc._coreDirOn[3] = true;
                if (_rockDelay == 0)
                {
                    _rockDelay = 7;
                    Skill();
                    _as.PlayOneShot(_ac);
                }
            }
            else
                if (_pc._coreDirOn[3]) _pc._coreDirOn[3] = false;
        }

        if (_rockDelay > 0)
        {
            _rockDelay -= Time.deltaTime;
            if (_rockDelay < 0) _rockDelay = 0;
        }

    }

    public void OnEventSkill()
    {
        _barrier.SetActive(true);
    }

    public void Skill()
    {
        _movingRockImage.SetActive(true);
        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        for (float i = 10; i > 0; i--)
        {
            _movingRockSR.DOFade(0.5f, i / 80);
            yield return new WaitForSeconds(i / 80);
            _movingRockSR.DOFade(0.8f, i / 80);
            yield return new WaitForSeconds(i / 80);
        }
        _movingRockSR.color = new Color(1, 0, 0, 0);
        _ChainImage.transform.position = _player.transform.position;
        _ChainImage.SetActive(true);
        GameManager.Instance._moving = false;
        Invoke("MovingUnrock", 2f);
        _movingRockImage.SetActive(false);
    }

    private void MovingUnrock()
    {
        _ChainImage.SetActive(false);
        GameManager.Instance._moving = true;
    }
}