using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int _score = 0;
    private static int _bestScore = 0;
    private static string _name = "무명";

    public int BestScore
    {
        get => _bestScore;
        set 
        {
            if (value > _bestScore)
            {
                _bestScore = value;
                UIManager.Instance.BestGameClear();
            }
        }
    }
    public string Name
    {
        get => _name;
        set
        {
            if (value != "")
            {
                _name = value;
            }
            else
            {
                _name = "무명";
            }
        }
    }

    [SerializeField]
    public GameObject _player;

    [SerializeField]
    GameObject _weapon;

    [SerializeField]
    private float _enemySpawnDelay = 0.5f;

    public bool _moving = true;
    public bool _GreenDie = false;

    public int _dieCoreCount = 0;

    private float _cameraSize;

    private void Awake()
    {
        Instance = this;
        _cameraSize = Camera.main.orthographicSize;
        CreatePoolManager();
    }

    [SerializeField]
    private PoolListSO _poolListSO;

    public void CreatePoolManager()
    {
        PoolManager.Instance = new PoolManager(transform);

        foreach (PoolingPair pair in _poolListSO.Pairs)
        {
            PoolManager.Instance.CreatePool(pair.prefab, pair.count);
        }
    }

    private void Start()
    {
        StartCoroutine(EnemySpawning(_enemySpawnDelay));
        TimeManager.Instance.TimeScale(1);
    }

    public void Score()
    {
        _score = (int)(46800f - TimeManager.Instance._allTime);
        BestScore = _score;
    }

    #region 무기 바꾸기
    public void WeaponChange(int index)
    {
        if (index == -1)
        {
            for (int i = 0; i < _weapon.transform.childCount && i < _player.transform.childCount; i++)
            {
                _player.transform.GetChild(i).gameObject.SetActive(false);
                _weapon.transform.GetChild(i).gameObject.SetActive(false);
            }
            UIManager.Instance.MagazineText("-", 0);
            UIManager.Instance.WeaponChangeUI();
        }
        else
        {
            for (int i = 0; i < _weapon.transform.childCount && i < _player.transform.childCount; i++)
            {
                if (i == index)
                {
                    _player.transform.GetChild(i).gameObject.SetActive(true);
                    _weapon.transform.GetChild(i).gameObject.SetActive(true);
                    continue;
                }
                _player.transform.GetChild(i).gameObject.SetActive(false);
                _weapon.transform.GetChild(i).gameObject.SetActive(false);
            }
            UIManager.Instance.WeaponChangeUI();
        }
    }
    #endregion

    public Vector3 CirclePos()
    {
        int r = 1 - Random.Range(0, 2) * 2;
        float x = Random.Range(-_cameraSize * 2, _cameraSize * 2);
        float y = Mathf.Pow(-Mathf.Pow(x, 2) + Mathf.Pow(_cameraSize * 2, 2), 0.5f) * r;

        Vector3 Pos = new Vector3(_player.transform.position.x + x, _player.transform.position.y + y, 0);

        return Pos;
    }

    IEnumerator EnemySpawning(float delay)
    {
        while (true)
        {
            PoolManager.Instance.Pop("Enemy", GameManager.Instance.CirclePos(), Vector2.zero, 0);
            yield return new WaitForSeconds(delay);
        }
    }
}
