using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI _myText;

    private void Awake()
    {
        _myText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_myText.text != $"최고점수\n{ScoreManager.Instance.ScoreLoad()._name} : {ScoreManager.Instance.ScoreLoad()._score:D2}")
            _myText.text = $"최고점수\n{ScoreManager.Instance.ScoreLoad()._name} : {ScoreManager.Instance.ScoreLoad()._score:D2}";
    }
}
