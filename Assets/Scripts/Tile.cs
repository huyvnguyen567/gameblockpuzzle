using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color originalColor; // Màu gốc của tile
    [SerializeField] private Color highlightColor; // Màu khi tile được highlight

    private void Start()
    {
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public void HighlightTile()
    {
        GetComponent<SpriteRenderer>().color = highlightColor;
    }

    public void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
