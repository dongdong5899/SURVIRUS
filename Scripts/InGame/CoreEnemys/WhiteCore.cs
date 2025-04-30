using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCore : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _otherCore = new List<GameObject>();

    private EnemyControler _ec;

    private float _skillDelay = 0;

    private string _sawnEnemy = "Enemy";
    private int _spawnCount = 5;

    private PlayerControler _pc;

    private void Awake()
    {
        _ec = GetComponent<EnemyControler>();
    }

    private void Start()
    {
        _pc = GameManager.Instance._player.gameObject.GetComponent<PlayerControler>();
    }

    private void Update()
    {
        if (_ec.Hp < 0.1f)
        {
            for (int i = 0; i < _otherCore.Count; i++)
            {
                if (!_otherCore[i].activeSelf)
                {
                    _otherCore[i].SetActive(true);
                    if (i < 3)
                        _otherCore[i].transform.position = new Vector3(Random.Range(0f, 70f), Random.Range(0f, 70f), 0);
                }
            }
            if (_pc._coreDirOn[4])
                _pc._coreDirOn[4] = false;
        }

        if (_skillDelay > 0)
        {
            _skillDelay -= Time.deltaTime;
            if (_skillDelay < 0)
                _skillDelay = 0;
        }
    }

    public void Skill()
    {
        if (_skillDelay == 0)
        {
            _skillDelay = 3f;
            StopCoroutine("Spawn");
            StartCoroutine("Spawn", _spawnCount);
        }
    }

    public void OnEventSkill()
    {
        _sawnEnemy = "EliteEnemy";
        _spawnCount = 3;
        Skill();
    }

    IEnumerator Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10;
            PoolManager.Instance.Pop(_sawnEnemy, transform.position, force, 0);
            yield return new WaitForSeconds(0.5f / count);
        }
    }
}
