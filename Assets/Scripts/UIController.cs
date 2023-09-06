using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private GameObject gamePlayWindowPrefab;
    [SerializeField] private GameObject gameOverWindowPrefab;
    [SerializeField] private GameObject mainMenuWindowPrefab;
    [SerializeField] private GameObject parentWindow;
    [SerializeField] private GameObject parentPopup;
    private GameObject gamePlayWindow;
    private GameObject gameOverWindow;
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
        gamePlayWindow.transform.SetParent(parentWindow.transform);
        gameOverWindow = Instantiate(gameOverWindowPrefab, parentPopup.gameObject.transform);
        gameOverWindow.transform.SetParent(parentPopup.transform);
        mainMenuWindow = Instantiate(mainMenuWindowPrefab, parentWindow.gameObject.transform);
        mainMenuWindow.transform.SetParent(parentWindow.transform);
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
                gameOverWindow.GetComponent<GameOverPopup>().ActiveGameOverWindow(isActive);
                break;
        }
    }
    public void UpdatePopup(PopupType type)
    {
        switch (type)
        {
            case PopupType.Gameover:
                gameOverWindow.GetComponent<GameOverPopup>().UpdateScoreText();
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
    Gameover
}