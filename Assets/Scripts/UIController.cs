using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private GameObject gamePlayWindowPrefab;
    [SerializeField] private GameObject gameOverWindowPrefab;
    [SerializeField] private GameObject parentWindow;
    [SerializeField] private GameObject parentPopup;
    private GameObject gamePlayWindow;
    private GameObject gameOverWindow;


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
    private void Start()
    {

    }
    private void SpawnUI()
    {
        gamePlayWindow = Instantiate(gamePlayWindowPrefab, parentWindow.gameObject.transform);
        gamePlayWindow.transform.SetParent(parentWindow.transform);
        gameOverWindow = Instantiate(gameOverWindowPrefab, parentPopup.gameObject.transform);
        gameOverWindow.transform.SetParent(parentPopup.transform);

    }

    public void ShowWindow(WindowType type, bool isActive)
    {
        switch (type)
        {
            case WindowType.Gameplay:
                gamePlayWindow.GetComponent<GameplayWindow>().ActiveGamePlayWindow(isActive);
                break;
            case WindowType.Gameover:
                gameOverWindow.GetComponent<GameOverWindow>().ActiveGameOverWindow(isActive); 
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
            case WindowType.Gameover:
                gameOverWindow.GetComponent<GameOverWindow>().UpdateScoreText();
                break;
        }
    }
}
public enum WindowType
{
    Gameplay, Gameover
}