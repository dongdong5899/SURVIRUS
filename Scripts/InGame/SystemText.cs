using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SystemText : MonoBehaviour
{
    TextMeshProUGUI _text;
    RectTransform _rt;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_rt.DOAnchorPos(Vector3.up * 350, 4f));
        seq.Join(_text.DOFade(0, 4f));
        seq.AppendCallback(() => Destroy(gameObject));
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            Destroy(gameObject);
        }
    }
}
