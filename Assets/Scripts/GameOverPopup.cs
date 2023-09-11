using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameOverPopup : MonoBehaviour
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

    public void ActiveGameOverPopup(bool isActive)
    {
        gameObject.SetActive(isActive);
        if(isActive)
            SoundManager.Instance.PlaySfx(SfxType.GameOver);
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + DataManager.Instance.Score;
    }

}
