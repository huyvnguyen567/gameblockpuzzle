using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController ins;
    public GameObject gameplayWindow;

    private void Awake()
    {
        if(ins != null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public void ShowWindow(WindowType type)
    //{
    //    switch (type)
    //    {
    //        case WindowType.Gameplay:

    //    }
    //}


}
public enum WindowType
{
    Gameplay
}