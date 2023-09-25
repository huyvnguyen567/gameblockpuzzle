using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Text changeCountText;
    [SerializeField] private Button btnPause;
    private void Awake()
    {
        SoundManager.Instance.PlayMusic(MusicType.GamePlay);
        UpdateChangeCount();
    }
    private void OnEnable()
    {
        UpdateScoreText();
        
        DataManager.Instance.LoadTile();
        DataManager.Instance.LoadTetrominoData();
    }
    public void ActiveGamePlayWindow(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void UpdateScoreText()
    {
        DataManager.Instance.LoadScore();
        scoreText.text = "Score: " + DataManager.Instance.Score;
        highScoreText.text = "High Score: " +DataManager.Instance.HighScore;
    }

    public void UpdateChangeCount()
    {
        changeCountText.text = DataManager.Instance.SwapQuantity.ToString();
    }
    public void OnClickPause()
    {
        if (!GameController.Instance.GameOver)
        {
            UIController.Instance.ShowPopup(PopupType.GamePause, true);
            GameController.Instance.GamePause = true;
        }
    }
    public void OnClickChangeTetromino()
    {
        if(DataManager.Instance.SwapQuantity > 0)
        {
            DataManager.Instance.SwapQuantity--;
            DataManager.Instance.SaveSwapQuantity();
            UpdateChangeCount();
            StartCoroutine(ChangeAndCheckGameOver());
        }
        else
        {
            Debug.Log("Bạn hết lượt đổi");
        }
    }
    private IEnumerator ChangeAndCheckGameOver()
    {
        GameController.Instance.ChangeTetromino();

        yield return new WaitForSeconds(1f);
        GameController.Instance.CheckGameOver();

    }
}
