using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    public float _reloadDelay = 0.7f;

    [SerializeField]
    private float _shootingDelay = 0.1f;
    private float _sd = 0f;

    [SerializeField]
    public int _oriMagazine = 30;
    [SerializeField]
    public int _magazine = 30;

    [SerializeField]
    private float _damege;
    [SerializeField]
    private float _knockBack;

    private PlayerControler _playerControler;
    private AudioSource _as;

    public bool _isReLoad = false;

    private float _reLogDelay = 0f;
    private float _buLogDelay = 0f;

    private void Awake()
    {
        _playerControler = GetComponentInParent<PlayerControler>();
        _as = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_reLogDelay > 0) _reLogDelay -= Time.deltaTime;
        if (_buLogDelay > 0) _buLogDelay -= Time.deltaTime;

        if (_sd < _shootingDelay) _sd += Time.deltaTime;
        if (Input.GetMouseButton(0) && Time.timeScale != 0)
            Bullet();
    }

    public void ReLoad()
    {
        _as.Play();
        StartCoroutine("ReLoadDel", _reloadDelay);
    }

    public void StopReLoad()
    {
        _isReLoad = false;
        StopCoroutine("ReLoadDel");
    }

    IEnumerator ReLoadDel(float time)
    {
        _isReLoad = true;
        UIManager.Instance.MagazineText("·", _oriMagazine);
        yield return new WaitForSeconds(time / 3);
        UIManager.Instance.MagazineText("··", _oriMagazine);
        yield return new WaitForSeconds(time / 3);
        UIManager.Instance.MagazineText("···", _oriMagazine);
        yield return new WaitForSeconds(time / 3);
        _magazine = _oriMagazine;
        UIManager.Instance.MagazineText(_magazine.ToString(), _oriMagazine);
        _isReLoad = false;
    }

    public void Bullet()
    {
        if (_sd >= _shootingDelay)
        {
            if (_isReLoad)
            {
                if (_reLogDelay <= 0)
                {
                    _reLogDelay = 1f;
                    UIManager.Instance.SystemText("장전중 입니다", Color.white);
                }
                return;
            }
            if (_magazine == 0)
            {
                if (_buLogDelay <= 0)
                {
                    _buLogDelay = 1f;
                    UIManager.Instance.SystemText("탄약이 없습니다", Color.white);
                }
                return;
            }
            PoolManager.Instance.Pop("Bullet", transform.position, transform.parent.GetComponent<PlayerControler>()._rotationMouse, _damege, _knockBack);
            CameraManager.Instance.CameraShake(0.2f, 8f);

            _magazine--;
            UIManager.Instance.MagazineText(_magazine.ToString(), _oriMagazine);

            _playerControler._bulletLight.GetComponent<Light>().Lighting();

            _sd = 0f;
        }
    }
}
