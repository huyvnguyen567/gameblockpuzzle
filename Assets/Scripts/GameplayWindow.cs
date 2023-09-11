using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button btnPause;
    private void Awake()
    {
        SoundManager.Instance.PlayMusic(MusicType.GamePlay);
    }
    public void ActiveGamePlayWindow(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " +DataManager.Instance.Score;
        highScoreText.text = "High Score: " +DataManager.Instance.HighScore;
    }
    public void OnClickPause()
    {
        if (!GameController.Instance.GameOver)
        {
            UIController.Instance.ShowPopup(PopupType.GamePause, true);
            UIController.Instance.UpdatePopup(PopupType.GamePause);
            GameController.Instance.GamePause = true;
        }
    }
}
