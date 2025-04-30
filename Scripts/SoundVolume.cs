using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolume : MonoBehaviour
{
    [SerializeField]
    private bool PlayOnAwake = false;

    [SerializeField]
    private float volume = 1;

    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _as.volume = SoundManager._soundValue * volume;
        if (PlayOnAwake)
            _as.Play();
    }

    private void Update()
    {
        if (_as.volume != SoundManager._soundValue * volume)
            _as.volume = SoundManager._soundValue * volume;
    }
}
