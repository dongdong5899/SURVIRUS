using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public static float _soundValue = 0.6f;

    private void Awake()
    {
        Instance = this;
    }
}
