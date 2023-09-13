using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private GameObject parentWindow;
    [SerializeField] private GameObject parentPopup;
    [SerializeField] private GameObject gamePlayWindowPrefab;
    [SerializeField] private GameObject mainMenuWindowPrefab;
    [SerializeField] private GameObject gameOverPopupPrefab;
    public GameObject scorePopupPrefab;
    [SerializeField] private GameObject gamePausePopupPrefab;
    private GameObject gamePlayWindow;
    private GameObject mainMenuWindow;
    private GameObject gameOverPopup;
    private GameObject gamePausePopup;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SpawnUI();
    }

   
    private void SpawnUI()
    {
        gamePlayWindow = Instantiate(gamePlayWindowPrefab, parentWindow.gameObject.transform);
        mainMenuWindow = Instantiate(mainMenuWindowPrefab, parentWindow.gameObject.transform);
        gameOverPopup = Instantiate(gameOverPopupPrefab, parentPopup.gameObject.transform);
        gamePausePopup = Instantiate(gamePausePopupPrefab, parentPopup.gameObject.transform);
    }

    public void ShowWindow(WindowType type, bool isActive)
    {
        switch (type)
        {
            case WindowType.GamePlay:
                gamePlayWindow.GetComponent<GameplayWindow>().ActiveGamePlayWindow(isActive);
                break;
            case WindowType.MainMenu:
                mainMenuWindow.GetComponent<MainMenuWindow>().ActiveMainMenuWindow(isActive);
                break;
        }
    }
    public void UpdateWindow(WindowType type)
    {
        switch (type)
        {
            case WindowType.GamePlay:
                gamePlayWindow.GetComponent<GameplayWindow>().UpdateScoreText();
                break;
            
        }
    }
    public void ShowPopup(PopupType type, bool isActive)
    {
        switch (type)
        {
            case PopupType.GameOver:
                gameOverPopup.GetComponent<GameOverPopup>().ActiveGameOverPopup(isActive);
                break;
            case PopupType.GamePause:
                gamePausePopup.GetComponent<GamePausePopup>().ActiveGamePausePopup(isActive);
                break;

        }
    }
    public void UpdatePopup(PopupType type)
    {
        switch (type)
        {
            case PopupType.GameOver:
                gameOverPopup.GetComponent<GameOverPopup>().UpdateScoreText();
                break;
            case PopupType.GamePause:
                gamePausePopup.GetComponent<GamePausePopup>().UpdateScoreText();
                break;
        }
    }
   
}
public enum WindowType
{
    GamePlay, MainMenu
}
public enum PopupType
{
    GameOver, Score, GamePause
}