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

        width = maxX + 1; // Add 1 because x is 0-based index
        height = maxY + 1; // Add 1 because y is 0-based index

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
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameController.Instance.GameOver)
            //Kiểm tra và đặt Tetromino vào lưới grid
            StartCoroutine(SnapAndCheckGameOver());
    }

    private IEnumerator SnapAndCheckGameOver()
    {
        SnapToValidGridCell();

        // Chờ cho tất cả các hàm khác trong SnapToValidGridCell hoàn thành
        yield return new WaitForSeconds(2);

        GameController.Instance.CheckGameOver();
    }
    private void SnapToValidGridCell()
    {
        bool canSnap = true;
        foreach (Transform tile in transform)
        {
            var origin = tile.position;
            RaycastHit2D hit = Physics2D.Raycast(origin, transform.forward, 10, layerMask);
            Debug.DrawRay(origin, Vector3.forward * 5, Color.red);
            if (hit.collider == null || !GameController.Instance.IsGridCellEmpty(tile.position))
            {
                canSnap = false;
            }
        }
        if (canSnap)
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
