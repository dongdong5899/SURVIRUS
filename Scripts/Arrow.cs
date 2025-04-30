using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private RectTransform _rt;
    [SerializeField]
    private AudioClip _ac;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }
    public void MouseEnter(RectTransform rt)
    {

        gameObject.SetActive(true);
        _rt.position = rt.position;
        rt.gameObject.GetComponent<AudioSource>().PlayOneShot(_ac);
    }

    public void MouseExit()
    {
        gameObject.SetActive(false);
    }
}
