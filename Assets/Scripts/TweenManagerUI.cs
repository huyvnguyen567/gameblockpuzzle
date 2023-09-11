using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenManagerUI : MonoBehaviour
{
    public static TweenManagerUI Instance;
    [SerializeField] private float duration = 1;
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
    public void OnScaleButton(RectTransform recTransform)
    {
        recTransform.DOScale(Vector3.zero, duration).From();
    }

    public void MoveYPopup(RectTransform rectTransform)
    {
        rectTransform.DOAnchorPosY(-800, duration, false);
    }

}
