using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tetromino : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float highlightDistance = 0.2f;
    private Vector3 initialPosition;
    private Vector3 offset = new Vector3(0, 3, 0);
    private bool isPlaced = false;

    private void Start()
    {
        initialPosition = transform.position;
    }
    private void Update()
    {
        //bool hightLightColor = true;
        //foreach (Transform tile in transform)
        //{
        //    var origin = tile.position;
        //    RaycastHit2D hit = Physics2D.Raycast(origin, transform.forward, 10, layerMask);
        //    Debug.DrawRay(origin, Vector3.forward * 5, Color.red);
        //    if (hit.collider == null || !GameController.Instance.IsGridCellEmpty(tile.position))
        //    {
        //        hightLightColor = false;
        //    }
        //}
        //if (hightLightColor)
        //{
        //    foreach (Transform tile in transform)
        //    {
        //        GameController.Instance.HightLightTile(tile.GetComponent<Tile>(), transform);
        //    }

        //}
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameController.Instance.IsPlaced(transform.gameObject))
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
        if (!GameController.Instance.IsPlaced(transform.gameObject))
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            target += offset;
            target.z = -1;
            transform.position = target;
        }
   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Kiểm tra và đặt Tetromino vào lưới grid
        SnapToValidGridCell();
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
            GameController.Instance.SnapTetrominoToGrid(transform);
            GameController.Instance.TetrominoUsed(transform.gameObject);
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
