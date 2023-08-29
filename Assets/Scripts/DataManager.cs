using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager ins;
    private int score;
    public int Score { 
        get { return score; }
        set {
            score = value;
        }
    }

    private void Awake()
    {
        if (ins != null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
