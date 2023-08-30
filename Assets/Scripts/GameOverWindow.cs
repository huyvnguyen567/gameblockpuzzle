using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void OnReplayClick()
    {
        GameController.Instance.ReplayGame();
    }

    public void ActiveGameOverWindow(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void UpdateScoreText()
    {
        scoreText.text = "Score: " +DataManager.Instance.Score;
    }
  
}
