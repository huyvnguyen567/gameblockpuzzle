using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    public static ScorePopup Create(Vector3 position, int scoreAmount)
    {
        Transform scorePopupTransform = Instantiate(UIController.Instance.scorePopupPrefab.transform, position, Quaternion.identity);
        ScorePopup scorePopup = scorePopupTransform.GetComponent<ScorePopup>();
        scorePopup.Setup(scoreAmount);
        return scorePopup;
    }
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }


    void Update()
    {
        float moveYSpeed = 2f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 0.5f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void Setup(int scoreAmount)
    {
        textColor = textMesh.color;
        textMesh.text = "+" + scoreAmount;
    }
  
}