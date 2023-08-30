using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] private int score = 0;
    [SerializeField] private int scorePerBlock = 2;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
        }
    }
    public int ScorePerBlock
    {
        get { return scorePerBlock; }
        set
        {
            scorePerBlock = value;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
