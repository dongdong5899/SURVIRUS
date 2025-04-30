using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float _allTime = 0;
    private float _time = 0;
    private int _mTime = 0, _hTime = 7, _day = 1;
    private float _nightLight = 0.1f;

    [SerializeField]
    private TextMeshProUGUI _timeText;

    [SerializeField]
    private Light2D _globalLight;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        GameTime();
    }

    public void TimeFreeze(float freezeValue, float beforeDelay, float freezeTime)
    {
        StartCoroutine(TimeFreezeCoroutine(freezeValue, beforeDelay, () =>
        {
            StartCoroutine(TimeFreezeCoroutine(1f, freezeTime));
        }));
    }

    IEnumerator TimeFreezeCoroutine(float freezeValue, float beforeDelay, Action Callback = null)
    {
        yield return new WaitForSecondsRealtime(beforeDelay);
        Time.timeScale = freezeValue;
        Callback?.Invoke();
    }

    private void GameTime()
    {
        _allTime += Time.deltaTime;
        _time += Time.deltaTime;
        if (_time >= 0.2f)
        {
            _time = 0;
            _mTime++;
            if (_mTime >= 60)
            {
                _mTime = 0;
                _hTime++;
                PoolManager.Instance.Pop("EliteEnemy", GameManager.Instance.CirclePos(), Vector2.zero, 0);
                if (_hTime >= 24)
                {
                    _hTime = 0;
                    _day++;
                    if (_day == 4)
                    {
                        GameManager.Instance.Score();
                        UIManager.Instance.GameOver();
                        TimeManager.Instance.TimeScale(0);
                    }
                }
            }
            _timeText.text = $"생존 {_day}일차\n{_hTime:D2}:{_mTime:D2}";
        }

        float time = _hTime * 60 + _mTime;


        if (1200 < time || time < 300)
            _globalLight.intensity = _nightLight;
        else if (time < 450)
            _globalLight.intensity = (time - 300) / 150 + _nightLight;
        else if (time < 750)
            _globalLight.intensity = time / 450 + _nightLight;
        else if (time < 1050)
            _globalLight.intensity = (1500 - time) / 450 + _nightLight;
        else
            _globalLight.intensity = (1200 - time) / 150 + _nightLight;
    }

    public void TimeScale(float value)
    {
        StopAllCoroutines();
        Time.timeScale = value;
    }
}
