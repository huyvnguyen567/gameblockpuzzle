using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width = 9, height = 9;
    [SerializeField] private Transform cam;
    [SerializeField] private List<Tile> listTiles = new List<Tile>();
    [SerializeField] private ObjectPooling poolTile;
    void Start()
    {
        CreateGrid();
        PerformRaycastForGrid();
    }

    private void CreateGrid()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tile = poolTile.GetObjectFromPool();

                if (tile != null)
                {
                    tile.transform.position = new Vector2(i,j);
                    tile.gameObject.SetActive(true);
                    tile.name = $"Tile {i} {j}";
                    tile.transform.SetParent(transform);
                    listTiles.Add(tile);
                }
            }
        }
        cam.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 2f, -10);
    }
    private void PerformRaycastForGrid()
    {
        foreach (Tile tile in listTiles)
        {
            Vector2 tilePosition = tile.transform.position;
            RaycastForTile(tilePosition);
        }
    }
    private void RaycastForTile(Vector2 tilePosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(tilePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Đã va chạm với một đối tượng
            // Thực hiện xử lý tại đây
        }
        else
        {
            // Không có va chạm
            // Thực hiện xử lý tại đây (nếu cần)
        }
    }
}
