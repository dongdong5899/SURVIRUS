using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private float _oriSpeed = 12;

    private float _speed;

    [SerializeField]
    private GameObject _dashParticle;

    [SerializeField]
    private GameObject[] _cores;
    private GameObject _dirs;

    public bool[] _coreDirOn = { false, false, false, false, false };

    [SerializeField]
    public GameObject _bulletLight;

    private Rigidbody2D _rb2d;
    private SpriteRenderer _sr;

    float _xMov = 0;
    float _yMov = 0;

    float _xRot = 0;
    float _yRot = 0;

    float _rotationMove = 0;
    public float _rotationMouse = 0;

    private GameObject _selectWeapon;
    private int _weaponIndex = 0;

    private ParticleSystem _hitEffect;
    private AudioSource _as;

    Vector3 _mousePos;
    public Vector3 _mouseDir;

    [SerializeField]
    private float _oriHp = 20f;
    [SerializeField]
    private float _oriStamina = 20f;

    [SerializeField]
    public Color _oriColor;

    private float _hp = 20f;
    private float _stamina = 20f;
    private bool _staminaH = true;
    private float _healStamina = 5f;

    private float _oriInv = 1.5f;
    private float _inv = 0;

    private KeyCode[] Alpha =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0
    };

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        _dirs = transform.Find("EnemyDir").gameObject;
        _hitEffect = transform.Find("HitEffect").GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _selectWeapon = transform.GetChild(_weaponIndex).gameObject;
        _hp = _oriHp;
        _stamina = _oriStamina;
        _speed = _oriSpeed;
    }

    void Update()
    {
        Move();
        Run();
        WeaponRot();
        WeaponSelect();
        ReLoad();
        Stamina();
        Hp();
        EnemyDir();
    }

    #region 움직임
    private void Move()
    {
        //마우스 위치
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;

        _mouseDir = transform.position - _mousePos;


        //플레이어 움직임
        if (GameManager.Instance._moving)
        {
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            _xMov = Input.GetAxisRaw("Horizontal");
            _yMov = Input.GetAxisRaw("Vertical");
            _rb2d.velocity = new Vector2(_xMov, _yMov).normalized * _speed;
        }
        else
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            _rb2d.velocity = Vector2.zero;
        }


        //마지막으로 움직인 방향
        if (_xMov != 0 || _yMov != 0)
        {
            _xRot = _xMov;
            _yRot = _yMov;
        }


        //각도
        _rotationMove = Mathf.Atan2(_yRot, _xRot) * Mathf.Rad2Deg - 90;
        _rotationMouse = Mathf.Atan2(_mouseDir.y, _mouseDir.x) * Mathf.Rad2Deg - 180;


        //빛 움직임
        _bulletLight.transform.position = transform.position - _mouseDir.normalized * 1.5f;
    }
    #endregion

    #region 달리기
    private void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _speed = _oriSpeed * 1.5f;
            _staminaH = false;
            UIManager.Instance.UsingRun(true);
        }
        if (Input.GetKey(KeyCode.Space) && !_staminaH)
        {
            _stamina -= 10 * Time.deltaTime;
            if (_stamina <= 0)
            {
                _speed = _oriSpeed;
                _staminaH = true;
                UIManager.Instance.UsingRun(false);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && !_staminaH)
        {
            _speed = _oriSpeed;
            _staminaH = true;
            UIManager.Instance.UsingRun(false);
        }
    }
    #endregion

    #region 무기 방향
    private void WeaponRot()
    {
        transform.GetChild(_weaponIndex).transform.rotation = Quaternion.Euler(new Vector3(0, 0, _rotationMouse));
        if (_mouseDir.x < 0)
            transform.GetChild(_weaponIndex).GetComponent<SpriteRenderer>().flipY = false;
        else
            transform.GetChild(_weaponIndex).GetComponent<SpriteRenderer>().flipY = true;
    }
    #endregion

    #region 무기 바꾸기
    //선택 방식
    private void WeaponSelect()
    {
        //스크롤 체인지
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (_selectWeapon.tag == "Gun" && _selectWeapon.GetComponent<Gun>()._isReLoad)
            {
                _selectWeapon.GetComponent<Gun>().StopReLoad();
            }
            _weaponIndex -= (int)(Input.GetAxis("Mouse ScrollWheel") * 10);
            _weaponIndex = (_weaponIndex + 9) % 3;
            WeaponChange();
        }
        //넘버키 체인지
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetKeyDown(Alpha[i]))
            {
                if (_selectWeapon.tag == "Gun" && _selectWeapon.GetComponent<Gun>()._isReLoad)
                {
                    _selectWeapon.GetComponent<Gun>().StopReLoad();
                }
                _weaponIndex = i;
                WeaponChange();
            }
        }
    }
    //무기 바꾸기
    private void WeaponChange()
    {
        GameManager.Instance.WeaponChange(_weaponIndex);
        _selectWeapon = transform.GetChild(_weaponIndex).gameObject;
        if (_selectWeapon.tag == "Gun")
            UIManager.Instance.MagazineText(_selectWeapon.GetComponent<Gun>()._magazine.ToString(), _selectWeapon.GetComponent<Gun>()._oriMagazine);
        if (_selectWeapon.tag == "Knife")
            UIManager.Instance.MagazineText("-", 0);
    }
    #endregion

    #region 장전
    private void ReLoad()
    {
        if (_selectWeapon.tag == "Gun" && !_selectWeapon.GetComponent<Gun>()._isReLoad)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _selectWeapon.GetComponent<Gun>().ReLoad();
            }
        }
    }
    #endregion

    #region 스테미나
    private void Stamina()
    {
        if (_stamina < _oriStamina && _staminaH)
        {
            _stamina += _healStamina * Time.deltaTime;
        }
        _stamina = Mathf.Clamp(_stamina, 0, _oriStamina);
        UIManager.Instance.StaminaUpdate(_stamina / _oriStamina);
    }
    #endregion

    #region 체력
    private void Hp()
    {
        UIManager.Instance.HpUpdate(_hp / _oriHp);
        _hp = Mathf.Clamp(_hp, 0, _oriHp);
        if (_inv > 0)
        {
            _inv -= Time.deltaTime;
        }
        _inv = Mathf.Clamp(_inv, 0, _oriInv);
    }

    public void HpDown(float damege)
    {
        if (_inv == 0)
        {
            _hp -= damege;
            _inv = _oriInv;
            StartCoroutine("Inv", _oriInv);
            _hitEffect.Play();
            _as.Play();
        }
        if (_hp <= 0)
        {
            UIManager.Instance.HpUpdate(_hp / _oriHp);
            UIManager.Instance.GameOver();
            TimeManager.Instance.TimeScale(0);
            gameObject.SetActive(false);
            DOTween.KillAll();
        }
    }
    public void HpUp(float heal)
    {
        _hp += heal;
    }

    IEnumerator Inv(float time)
    {
        for (int i = 0; i < 5; i++)
        {
            _sr.DOColor(Color.red, time / 10);
            yield return new WaitForSeconds(time / 10);
            _sr.DOColor(_oriColor, time / 10);
            yield return new WaitForSeconds(time / 10);
        }
    }
    #endregion

    private void EnemyDir()
    {
        Vector2 dir;
        for (int i = 0; i < 5; i++)
        {
            dir = _cores[i].transform.position - transform.position;
            _dirs.transform.GetChild(i).transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
            _dirs.transform.GetChild(i).gameObject.SetActive(_coreDirOn[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item._itemType == ItemType.Healing)
            {
                PoolManager.Instance.Push(item);
                PoolManager.Instance.Pop("HLight", item.transform.position);
                HpUp(2f);
            }
        }
    }
}