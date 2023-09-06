using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : MonoBehaviour
{
    
    public void OnStartClick()
    {
        SceneManager.LoadScene("Game Play");
        UIController.Instance.ShowWindow(WindowType.Mainmenu,false);
    }
    public void OnQuitClick()
    {
        Debug.Log("Thoat game");
        Application.Quit();
    }
    public void ActiveMainMenuWindow(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
