using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private GameObject gamePlayWindowPrefab;
    [SerializeField] private GameObject gameOverPopupPrefab;
    [SerializeField] private GameObject mainMenuWindowPrefab;
    public GameObject scorePopupPrefab;
    [SerializeField] private GameObject parentWindow;
    [SerializeField] private GameObject parentPopup;
    private GameObject gamePlayWindow;
    private GameObject gameOverPopup;
    private GameObject mainMenuWindow;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
        gameOverPopup = Instantiate(gameOverPopupPrefab, parentPopup.gameObject.transform);
        mainMenuWindow = Instantiate(mainMenuWindowPrefab, parentWindow.gameObject.transform);
    }

    public void ShowWindow(WindowType type, bool isActive)
    {
        switch (type)
        {
            case WindowType.Gameplay:
                gamePlayWindow.GetComponent<GameplayWindow>().ActiveGamePlayWindow(isActive);
                break;
            case WindowType.Mainmenu:
                mainMenuWindow.GetComponent<MainMenuWindow>().ActiveMainMenuWindow(isActive);
                break;
        }
    }
    public void UpdateWindow(WindowType type)
    {
        switch (type)
        {
            case WindowType.Gameplay:
                gamePlayWindow.GetComponent<GameplayWindow>().UpdateScoreText();
                break;
            
        }
    }
    public void ShowPopup(PopupType type, bool isActive)
    {
        switch (type)
        {
            case PopupType.Gameover:
                gameOverPopup.GetComponent<GameOverPopup>().ActiveGameOverWindow(isActive);
                TweenManagerUI.Instance.MoveYPopup(gameOverPopup.gameObject.GetComponent<RectTransform>());
                break;
     
        }
    }
    public void UpdatePopup(PopupType type)
    {
        switch (type)
        {
            case PopupType.Gameover:
                gameOverPopup.GetComponent<GameOverPopup>().UpdateScoreText();
                break;
 
        }
    }
   
}
public enum WindowType
{
    Gameplay, Mainmenu
}
public enum PopupType
{
    Gameover, Score
}