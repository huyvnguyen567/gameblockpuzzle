﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tetromino : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float highlightDistance = 0.2f;
    [SerializeField] private Vector3 offset = new Vector3(0, 3, 0);
    private Vector3 initialPosition;
    private int height;
    private int width;
    public int Height => height;
    public int Width => width;

    private void Awake()
    {
        initialPosition = transform.position;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (Transform tile in transform)
        {
            int x = Mathf.RoundToInt(tile.localPosition.x);
            int y = Mathf.RoundToInt(tile.localPosition.y);

            if (x > maxX)
            {
                maxX = x;
            }

            if (y > maxY)
            {
                maxY = y;
            }
        }

        width = maxX + 1; 
        height = maxY + 1; 

        //Debug.Log(width + " " + height + " " + name);

    }
    private void Start()
    {
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameController.Instance.IsPlaced(transform.gameObject) && !GameController.Instance.GameOver)
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            target += offset;
            target.z = -1;
            transform.position = target;
            transform.localScale = new Vector3(1, 1, 1);
        }
 
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!GameController.Instance.IsPlaced(transform.gameObject) && !GameController.Instance.GameOver)
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            target += offset;
            target.z = -1;
            transform.position = target;
            HighLightColor();

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach(Transform tile in GameController.Instance.gridContainer)
        {
            tile.GetComponent<Tile>().ResetColor();
        }
        
        if (!GameController.Instance.GameOver) 
        {
            //Kiểm tra và đặt Tetromino vào lưới grid
            StartCoroutine(SnapAndCheckGameOver());
        }
            
    }

    public void HighLightColor()
    {
        
        if (CanSnap())
        {
            foreach (Transform tile in transform)
            {
                var origin = tile.position;
                RaycastHit2D hit = Physics2D.Raycast(origin, transform.forward, 10, layerMask);
                if (hit.collider == null || !GameController.Instance.IsGridCellEmpty(tile.position))
                {
                    
                }
                else
                {
                    hit.collider.GetComponent<Tile>().HighlightTile();
                }
            }
        }
        else
        {
            foreach (Transform tile in GameController.Instance.gridContainer)
            {
                tile.GetComponent<Tile>().ResetColor();
            }
        }

       


    }

    public bool CanSnap()
    {
        foreach (Transform tile in transform)
        {
            var origin = tile.position;
            RaycastHit2D hit = Physics2D.Raycast(origin, transform.forward, 10, layerMask);
            if (hit.collider == null || !GameController.Instance.IsGridCellEmpty(tile.position))
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerator SnapAndCheckGameOver()
    {
        SnapToValidGridCell();

        // Chờ cho tất cả các hàm khác trong SnapToValidGridCell hoàn thành
        yield return new WaitForSeconds(0.5f);

        GameController.Instance.CheckGameOver();
    }
    private void SnapToValidGridCell()
    {  
        if (CanSnap())
        {
            GameController.Instance.IncreaseScore(transform.childCount);
            GameController.Instance.SnapTetrominoToGrid(transform);
            GameController.Instance.TetrominoUsed(transform.gameObject);
            GameController.Instance.CheckAndClearFullColumns();
            GameController.Instance.CheckAndClearFullRows();
            for (int x = 0; x < GameController.Instance.Width; x += 3)
            {
                for (int y = 0; y < GameController.Instance.Height; y += 3)
                {
                    GameController.Instance.CheckAndClearSquare(x, y);
                }
            }
        }
        else
        {
            if (!GameController.Instance.IsPlaced(transform.gameObject))
            {
                transform.localScale = new Vector3(0.6f, 0.6f, 1);
                transform.position = initialPosition;
            }
        } 
    }
}
