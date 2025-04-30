using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Diff : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (DifficultyManager.Instance._difficultyName != _text.text)
        {
            _text.text = DifficultyManager.Instance._difficultyName;
        }
    }
}
