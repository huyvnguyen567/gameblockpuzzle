using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    //[SerializeField] private GameObject backgroundGamePlay;

 
    public void ActiveGamePlayWindow(bool isActive)
    {
        gameObject.SetActive(isActive);
        //backgroundGamePlay.SetActive(isActive);
    }
    public void UpdateScoreText()
    {
        scoreText.text = DataManager.Instance.Score + "" ;
    }
}
