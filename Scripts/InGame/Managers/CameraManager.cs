using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] Camera _cameraBrain;

    [SerializeField] CinemachineVirtualCamera _camera;
    CinemachineBasicMultiChannelPerlin _cameraBasic;

    private Tween _ShakeTween;

    private void Awake()
    {
        Instance = this;
        _cameraBasic = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    

    public void CameraShake(float time, float power)
    {
        if (_ShakeTween != null && _ShakeTween.IsActive())
            _ShakeTween.Kill();
        _cameraBasic.m_AmplitudeGain = power;
        _ShakeTween = DOTween.To(
            () => _cameraBasic.m_AmplitudeGain,
            value => _cameraBasic.m_AmplitudeGain = value,
            0, time);
    }

    public void TargetChange(Transform target)
    {
        _camera.m_Follow = target;
    }
}
