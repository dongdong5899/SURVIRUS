using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject _magazineText;

    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private GameObject _systemText;

    Sequence _seq;

    private Image _runing;
    private Image _stamina;
    private Image _hp;
    private GameObject _gameOverMenu;
    private GameObject _gameClearMenu;
    private GameObject _bestGameClearSet;
    public TMP_InputField _inputName;
    private GameObject _escMenu;
    private GameObject _optionMenu;
    private TextMeshProUGUI _diffText;

    private void Awake()
    {
        Instance = this;
        _runing = _canvas.transform.Find("Skill/Run").GetComponent<Image>();
        _stamina = _canvas.transform.Find("StaminaBar/Stamina").GetComponent<Image>();
        _hp = _canvas.transform.Find("HpBar/Hp").GetComponent<Image>();
        _gameOverMenu = _canvas.transform.Find("GameOverMenu").gameObject;
        _gameClearMenu = _canvas.transform.Find("GameClearMenu").gameObject;
        _bestGameClearSet = _gameClearMenu.transform.Find("BestName").gameObject;
        _inputName = _bestGameClearSet.transform.Find("NameInput").GetComponent<TMP_InputField>();
        _escMenu = _canvas.transform.Find("ESC").gameObject;
        _optionMenu = _canvas.transform.Find("Option").gameObject;
        _diffText = _canvas.transform.Find("Diff").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _diffText.text = DifficultyManager.Instance._difficultyName;
    }

    private void Update()
    {
        if (_inputName.transform.gameObject.activeSelf)
        {
            GameManager.Instance.Name = _inputName.text;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_optionMenu.activeSelf)
            {
                _optionMenu.SetActive(false);
                if (!_gameOverMenu.activeSelf)
                {
                    TimeManager.Instance.TimeScale(1f);
                }
            }
            else
            {
                EscMenu(!_escMenu.activeSelf);
            }
        }
    }

    public void EscMenu(bool active)
    {
        _escMenu.SetActive(active);
        if (_escMenu.activeSelf)
        {
            TimeManager.Instance.TimeScale(0f);
        }
        else if (!_gameOverMenu.activeSelf)
        {
            TimeManager.Instance.TimeScale(1f);
        }
    }

    public void OptionMenu()
    {
        _optionMenu.SetActive(true);
        _escMenu.SetActive(false);
    }

    public void MagazineText(string magazine, int oriMagazine)
    {
        _magazineText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = magazine;
        _magazineText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"/ {oriMagazine}";
    }

    public void SystemText(string text, Color color)
    {
        TextMeshProUGUI t = Instantiate(_systemText, _canvas.transform).GetComponent<TextMeshProUGUI>();
        t.text = text;
        t.color = color;
    }

    public void WeaponChangeUI()
    {
        _seq = DOTween.Sequence();
        _seq.Append(_canvas.transform.Find("Weapon").transform.GetComponent<Image>().DOFade(0.8f, 0.07f));
        _seq.Append(_canvas.transform.Find("Weapon").transform.GetComponent<Image>().DOFade(0.4f, 0.07f));
    }

    public void UsingRun(bool isUse)
    {
        if (isUse)
        {
            _runing.color = Color.green;
        }
        else
        {
            _runing.color = Color.white;
        }
    }

    public void StaminaUpdate(float stamina)
    {
        _stamina.fillAmount = stamina;
    }

    public void HpUpdate(float Hp)
    {
        _hp.fillAmount = Hp;
    }

    public void GameOver()
    {
        _gameOverMenu.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"점수 : -";
        _gameOverMenu.SetActive(true);
    }
    public void GameClear()
    {
        _gameClearMenu.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"점수 : {GameManager.Instance._score}";
        _gameClearMenu.SetActive(true);
    }
    public void BestGameClear()
    {
        _bestGameClearSet.SetActive(true);
    }
}
