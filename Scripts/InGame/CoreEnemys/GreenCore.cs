using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCore : MonoBehaviour
{
    private EnemyControler _ec;
    private GameObject _player;
    private GameObject _circle;

    private PlayerControler _pc;

    private void Awake()
    {
        _ec = GetComponent<EnemyControler>();
        _circle = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        _player = GameManager.Instance._player;
        _pc = _player.GetComponent<PlayerControler>();
    }

    private void Update()
    {
        if (_ec.Hp < 0.1f)
        {
            if (_pc._coreDirOn[2]) _pc._coreDirOn[2] = false;
            StopAllCoroutines();
            _circle.SetActive(false);
            GameManager.Instance._GreenDie = true;
        }
        else
        {
            Vector3 dir = _player.transform.position - transform.position;
            if (Mathf.Abs((dir).magnitude) < 40f)
            {
                if (!_pc._coreDirOn[2]) _pc._coreDirOn[2] = true;
            }
            else
                if (_pc._coreDirOn[2]) _pc._coreDirOn[2] = false;
        }
    }

    public void HpDown(float damage)
    {
        _ec.Hp -= damage;
        Debug.Log("체력 다운");
    }

    public void OnEventSkill()
    {
        _circle.transform.localScale = new Vector3(8, 8, 8);
        StartCoroutine("CircleMove");
    }

    IEnumerator CircleMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            float x = Random.Range(-10f, 10);
            float y = Random.Range(-7f, 7);
            _circle.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, 0);
        }
    }
}
