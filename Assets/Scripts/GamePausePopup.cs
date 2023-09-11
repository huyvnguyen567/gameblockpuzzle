using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePausePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void OnReplayClick()
    {
        GameController.Instance.ReplayGame();
    }
    public void OnMainMenuClick()
    {
        GameController.Instance.LoadMainMenu();
    }
    public void OnClosePopup()
    {
        UIController.Instance.ShowPopup(PopupType.GamePause, false);
        GameController.Instance.GamePause = false;
    }
    public void ActiveGamePausePopup(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + DataManager.Instance.Score;
    }
}
