using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEscManager : MonoBehaviour
{
    public static MainEscManager Instance;

    [SerializeField]
    private GameObject _canvas;
    private GameObject _tutorials;
    private GameObject _tutorialsE;
    private GameObject _tutorialsK;
    private GameObject _tutorialsButton;
    private GameObject _esc;
    private GameObject _option;

    private void Awake()
    {
        Instance = this;
        _esc = _canvas.transform.Find("ESC").gameObject;
        _option = _canvas.transform.Find("Option").gameObject;
        _tutorials = _canvas.transform.Find("TutorialsBack").gameObject;
        _tutorialsE = _canvas.transform.Find("Enemy_T_Back").gameObject;
        _tutorialsK = _canvas.transform.Find("Key_T_Back").gameObject;
        _tutorialsButton = _canvas.transform.Find("TutorialsButton").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_option.activeSelf)
            {
                _option.SetActive(false);
            }
            else if (_tutorialsE.activeSelf)
            {
                _tutorialsE.SetActive(false);
            }
            else if (_tutorialsK.activeSelf)
            {
                _tutorialsK.SetActive(false);
            }
            else if (_tutorials.activeSelf)
            {
                _tutorials.SetActive(false);
            }
            else
            {
                _esc.SetActive(!_esc.activeSelf);
            }
            _tutorialsButton.SetActive(!_esc.activeSelf);
        }
    }

    public void OptionMenu()
    {
        _esc.SetActive(false);
        _option.SetActive(true);
    }
    public void ExitOptionMenu()
    {
        _option.SetActive(false);
    }
}
