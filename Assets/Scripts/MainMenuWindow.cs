using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private RectTransform[] scaleTransforms;

    private void Awake()
    {
        SoundManager.Instance.PlayMusic(MusicType.MainMenu);
    }
    private void Start()
    {
        foreach(RectTransform scaleTransform in scaleTransforms)
        {
            TweenManagerUI.Instance.OnScaleButton(scaleTransform);
        }
    }
    public void OnStartClick()
    {
        SoundManager.Instance.PlaySfx(SfxType.ButtonClick);
        SceneManager.LoadScene("Game Play");
    }
    public void OnQuitClick()
    {
        SoundManager.Instance.PlaySfx(SfxType.ButtonClick);
        Debug.Log("Thoat game");
        Application.Quit();
    }
    public void ActiveMainMenuWindow(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
