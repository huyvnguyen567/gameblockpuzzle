using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWindow : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public void UpdateScoreText()
    {
        //scoreText.text = GameController.Instance.score;
    }
}
