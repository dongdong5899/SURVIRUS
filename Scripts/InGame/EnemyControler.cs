using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using DG.Tweening;


public enum EnemyType
{
    Defalt,
    Elite,
    Core
}


public enum CoreType
{
    Defalt,
    White,
    Red,
    Green,
    Blue,
    Main
}

public class EnemyControler : PoolableMono
{

    [SerializeField]
    private float _oriSpeed = 4;

    [SerializeField]
    private float _knockBackRes = 1;

    [SerializeField]
    private float _oriHp = 10;

    [SerializeField]
    private GameObject _dieParticle;

    [SerializeField]
    private float _damege = 2;

    [SerializeField]
    public EnemyType _enemyType;

    [SerializeField]
    public CoreType _coreType;

    private float _hp = 10;
    public float Hp
    {
        get => _hp;
        set => _hp = value > 0 ? value : 0;
    }

    private GameObject _target;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc2d;
    private Rigidbody2D _rb2d;
    private ShadowCaster2D _sc2d;
    private EnemyControler _greenCore;
    private AudioSource _as;

    private GameObject _hpBarObject;
    private Image _hpBar;
    [SerializeField]
    private Color _oriHpBarColor;
    private Image _hpDownBar;
    private float _downBarDelay = 0f;

    [SerializeField]
    private Color _oriColor;
    private float _speed;

    Vector3 _dir;

    private bool _inGreenCircle = false;
    private bool S = false;

    public override void Init()
    {
        _inGreenCircle = false;
        _hp = _oriHp * DifficultyManager.Instance.Hp;
        _sr.color = _oriColor;
        _speed = _oriSpeed;
        _hpBar.color = _oriHpBarColor;
        _hpBar.fillAmount = 1f;
        _hpDownBar.fillAmount = 1f;
        _sr.sortingOrder = 1;
        _sc2d.enabled = true;
        _bc2d.enabled = false;
        _hpBarObject.SetActive(false);
        Invoke("CollisionOn", 0.35f);
    }

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _bc2d = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _sc2d = GetComponent<ShadowCaster2D>();
        _as = GetComponent<AudioSource>();
        _hpBarObject = transform.Find("EnemyHpBar").gameObject;
        _hpBar = _hpBarObject.transform.Find("Back/HpBar").GetComponent<Image>();
        _hpDownBar = _hpBarObject.transform.Find("Back/HpDownBar").GetComponent<Image>();

    }

    private void Start()
    {
        _target = GameManager.Instance._player;
        _hpBar.color = _oriHpBarColor;
        _hp = _oriHp * DifficultyManager.Instance.Hp;
        _sr.color = _oriColor;
    }

    void Update()
    {
        if (_downBarDelay > 0) _downBarDelay -= Time.deltaTime;
        if (_downBarDelay < 0)
        {
            StopCoroutine("HpDownBar");
            StartCoroutine("HpDownBar", _hpBar.fillAmount);
            _downBarDelay = 0;
        }
        _hpBar.color = Color.Lerp(Color.red, _oriColor, _hpBar.fillAmount);

        _dir = (_target.transform.position - transform.position).normalized;

        if (_hp > 0)
        {
            Move();
        }

        if (GameManager.Instance._GreenDie && _inGreenCircle)
        {
            _inGreenCircle = false;
        }
    }

    #region 움직임
    private void Move()
    {
        _rb2d.velocity += _rb2d.velocity.x * Mathf.Sign(_dir.x) <= _dir.x * _speed * Mathf.Sign(_dir.x)
            ? new Vector2(_dir.x / 5, 0)
            : new Vector2(-_dir.x / 5, 0);

        _rb2d.velocity += _rb2d.velocity.y * Mathf.Sign(_dir.y) <= _dir.y * _speed * Mathf.Sign(_dir.y)
            ? new Vector2(0, _dir.y / 5)
            : new Vector2(0, -_dir.y / 5);
        //수정 필요! 무조건 수정! 더 깔끔하게!
    }
    #endregion

    #region 피격
    public void HpDown(float damage, Vector2 knockBackDir, float knockBackPower)
    {
        _downBarDelay = 0.5f;
        _hp -= damage;
        _hpBar.fillAmount = _hp / (_oriHp * DifficultyManager.Instance.Hp);
        _as.Play();

        if (!_hpBarObject.activeSelf) _hpBarObject.SetActive(true);

        KnockBack(knockBackDir, knockBackPower);
        if (_hp > 0)
        {
            _sr.color = new Color(1, 0, 0);
            _sr.DOKill();
            _sr.DOColor(_oriColor, 0.3f);

            if (_coreType == CoreType.White)
            {
                GetComponent<WhiteCore>().Skill();
            }
            else if (_coreType == CoreType.Main)
            {
                GetComponent<MainCore>().WhiteSkill();
            }
            CoreSkill(_coreType);
        }
        else
        {
            _bc2d.enabled = false;
            _sc2d.enabled = false;
            _sr.DOKill();
            _sr.color = new Color(0.3f, 0f, 0f);
            _sr.sortingOrder = 0;
            _rb2d.velocity = Vector2.zero;
            _hpBarObject.SetActive(false);
            PoolManager.Instance.Pop("DieParticle", transform.position);
            Invoke("Die", 30f);

            if (_inGreenCircle)
            {
                _greenCore.HpDown(10f, Vector2.zero, 1f);
            }

            if (_enemyType == EnemyType.Core)
            {
                TimeManager.Instance.TimeFreeze(freezeValue: 0.2f, beforeDelay: 0.1f, freezeTime: 2f);
                if (_coreType == CoreType.Main)
                {
                    Invoke("Clear", 0.5f);
                }
                StartCoroutine(CameraFollow());
                GameManager.Instance._dieCoreCount++;
            }
            else if (_enemyType == EnemyType.Elite)
            {
                if (Random.Range(1, 3) == 1)
                    PoolManager.Instance.Pop("HealingItem", transform.position);
            }
        }
    }

    private void Clear()
    {
        GameManager.Instance.Score();
        UIManager.Instance.GameClear();
        TimeManager.Instance.TimeScale(0);
        DOTween.KillAll();
    }

    private void KnockBack(Vector2 knockBack, float knockBackPower)
    {
        _rb2d.velocity = knockBack * (knockBackPower / _knockBackRes - _knockBackRes / knockBackPower);
    }

    private void Die()
    {
        if (_enemyType == EnemyType.Core) gameObject.SetActive(false);
        else PoolManager.Instance.Push(this);
    }
    #endregion

    IEnumerator HpDownBar(float hp)
    {
        float startDHp = _hpDownBar.fillAmount;
        float current = 0f;
        float persant = 0f;
        while(persant < 1f)
        {
            current += Time.deltaTime;
            persant = current / 0.5f;
            _hpDownBar.fillAmount = Mathf.Lerp(startDHp, hp, persant);
            yield return null;
        }
    }

    #region 이벤트 스킬
    private void CoreSkill(CoreType core)
    {
        if (_hp / (_oriHp * DifficultyManager.Instance.Hp) < 0.3f)
        {
            if (!S)
            {
                S = true;
                if (core != CoreType.Defalt)
                    UIManager.Instance.SystemText($"\"{core}\"의 경계 강화됩니다!", Color.red);
                switch (core)
                {
                    case CoreType.White:
                        GetComponent<WhiteCore>().OnEventSkill();
                        break;
                    case CoreType.Red:
                        GetComponent<RedCore>().OnEventSkill();
                        break;
                    case CoreType.Green:
                        GetComponent<GreenCore>().OnEventSkill();
                        break;
                    case CoreType.Blue:
                        GetComponent<BlueCore>().OnEventSkill();
                        break;
                    case CoreType.Main:
                        GetComponent<MainCore>().OnEventSkill();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #endregion

    IEnumerator CameraFollow()
    {
        CameraManager.Instance.TargetChange(transform);
        yield return new WaitForSecondsRealtime(2f);
        CameraManager.Instance.TargetChange(_target.transform);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControler>().HpDown(_damege * DifficultyManager.Instance.Dm);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GreenBarrier"))
        {
            _inGreenCircle = true;
            _greenCore = collision.gameObject.GetComponentInParent<EnemyControler>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GreenBarrier") && _bc2d.enabled)
        {
            _inGreenCircle = false;
        }
    }

    private void CollisionOn()
    {
        _bc2d.enabled = true;
    }
}
