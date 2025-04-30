using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum s
{
    Sd,
    sd
}
public class ButtonManager : MonoBehaviour
{
    #region 매인화면
    public void StartButtonDown()
    {
        SceneManager.LoadScene("SurvivalScene");
        Time.timeScale = 1;
    }

    public void MainOptionMenuButtonDown()
    {
        MainEscManager.Instance.OptionMenu();
    }

    public void GameEnd()
    {
        Application.Quit();
    }

    public void ScoreResetButtonDown()
    {
        ScoreManager.Instance.ScoreSave("무명", 0);
    }

    public void TEsc(GameObject _target)
    {
        _target.SetActive(false);
    }
    public void TIn(GameObject _target)
    {
        _target.SetActive(true);
    }
    #endregion

    #region 인게임
    public void PlayButtonDown()
    {
        UIManager.Instance.EscMenu(false);
    }

    public void InGameOptionButtonDown()
    {
        UIManager.Instance.OptionMenu();
    }

    public void OutButtonDown()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }

    public void RePlayButtonDown()
    {
        SceneManager.LoadScene("SurvivalScene");
        if (GameManager.Instance._score >= GameManager.Instance.BestScore)
            ScoreManager.Instance.ScoreSave(GameManager.Instance.Name, GameManager.Instance.BestScore);
        Time.timeScale = 1;
    }

    public void MainButtonDown()
    {
        SceneManager.LoadScene("MainScene");
        if (GameManager.Instance._score >= GameManager.Instance.BestScore)
            ScoreManager.Instance.ScoreSave(GameManager.Instance.Name, GameManager.Instance.BestScore);
        Time.timeScale = 1;
    }
    #endregion
}
