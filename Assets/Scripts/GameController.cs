using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] private Transform gridContainer; // Container chứa các ô trong lưới grid
    [SerializeField] private GameObject tilePrefab; // Prefab của ô (tile)
    [SerializeField] private int width = 9;
    [SerializeField] private int height = 9;
    [SerializeField] private List<GameObject> tetrominoPrefabs;
    [SerializeField] private Transform[] spawnPositions;
    private List<GameObject> currentTetrominos = new List<GameObject>();
    private List<Tile> listTiles = new List<Tile>();
    public Transform[,] grid; // Lưới grid lưu trữ thông tin về ô


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Khởi tạo lưới grid và các thông tin liên quan
        InitializeGrid();
        // Sinh ngẫu nhiên các khối tetromino
        SpawnInitialTetrominos();

    }
    private void Update()
    {
        CheckAndClearFullColumns();
        CheckAndClearFullRows();
        //CheckAndClearSquare(3,3);
    }
    private void InitializeGrid()
    {
        grid = new Transform[width, height]; // Khởi tạo lưới grid có kích thước 10x20

        // Lặp qua tất cả các ô trong lưới grid để khởi tạo các ô
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Tạo GameObject từ prefab của ô (tile)
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                newTile.name = $"Tile {x} {y}";
                newTile.transform.SetParent(gridContainer);
                listTiles.Add(newTile.GetComponent<Tile>());

                // Lưu trữ transform của ô trong lưới grid
                grid[x, y] = newTile.transform;
            }
        }
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 2f, -10);
    }
   
    public Vector3 CalculateGridPosition(Vector3 position)
    {
        return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }


    public bool IsGridCellEmpty(Vector3 gridPosition)
    {
        int x = (int)Mathf.Round(gridPosition.x);
        int y = (int)Mathf.Round(gridPosition.y);

        // Kiểm tra xem x và y có nằm trong phạm vi hợp lệ của mảng không
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
        {
            if (grid[x, y] != null)
            {
                if (grid[x, y].gameObject.CompareTag("TileShape"))
                {
                    return false;
                }
            }
        }
        else
        {
            return false; // Nếu nằm ngoài phạm vi mảng, coi như ô không trống
        }
        return true;
    }

    public void PlaceTileInGrid(Vector3 gridPosition, Transform tileTransform)
    {
        int x = (int)Mathf.Round(gridPosition.x);
        int y = (int)Mathf.Round(gridPosition.y);

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y] = tileTransform;
        }
    }

    public void SnapTetrominoToGrid(Transform tetromino)
    {
        foreach (Transform tile in tetromino)
        {
            Vector3 gridPosition = CalculateGridPosition(tile.position);
            gridPosition.z = -1;
            PlaceTileInGrid(gridPosition, tile);
            tile.position = gridPosition;
        }
    }
    private void SpawnInitialTetrominos()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            GameObject tetrominoPrefab = GetRandomTetrominoPrefab();
            Vector3 spawnPosition = spawnPositions[i].position;
            GameObject tetromino = Instantiate(tetrominoPrefab, spawnPosition, Quaternion.identity);
            currentTetrominos.Add(tetromino);
        }
    }

    private GameObject GetRandomTetrominoPrefab()
    {
        int randomIndex = Random.Range(0, tetrominoPrefabs.Count);
        return tetrominoPrefabs[randomIndex];
    }

    // Gọi hàm này khi Tetromino đã được drop
    public void TetrominoUsed(GameObject tetromino)
    {
        currentTetrominos.Remove(tetromino);

        if (currentTetrominos.Count == 0)
        {
            // Sinh ngẫu nhiễn các tetromino mới
            SpawnInitialTetrominos();
        }
    }

    public bool IsPlaced(GameObject tetromino)
    {
        if (currentTetrominos.Contains(tetromino))
        {
            return false;
        }
        return true;
    }

    public void CheckAndClearFullRows()
    {
        for (int y = 0; y < height; y++)
        {
            bool isRowFull = true;
            for(int x = 0; x < width; x++)
            {
                Transform tile = grid[x, y];
                if (tile == null || !tile.CompareTag("TileShape"))
                {
                    isRowFull = false;
                    break;
                }
            }
            if (isRowFull)
            {
                Debug.Log("full dòng " +y);
                //Xóa dòng
                for (int x = 0; x < width; x++)
                {
                    Transform tile = grid[x, y];
                    if (tile != null && tile.CompareTag("TileShape"))
                    {
                        Destroy(tile.gameObject);
                    }
                }
            }
        }
    }
    public void CheckAndClearFullColumns()
    {
        for (int x = 0; x < height; x++)
        {
            bool isColumnFull = true;
            for (int y = 0; y < width; y++)
            {
                Transform tile = grid[x, y];
                if (tile == null || !tile.CompareTag("TileShape"))
                {
                    isColumnFull = false;
                    break;
                }
            }
            if (isColumnFull)
            {
                Debug.Log("full cột " + x);

                //Xóa cột
                for (int y = 0; y < height; y++)
                {
                    Transform tile = grid[x, y];
                    if (tile != null && tile.CompareTag("TileShape"))
                    {
                        Destroy(tile.gameObject);
                    }
                }
            }
        }
    }
    //public void CheckAndClearSquare(int startX, int startY)
    //{
    //    bool isSquareFull = true;
    //    for(int x = startX; x < startX + 3; x++)
    //    {
    //        for(int y = startY; y < startY + 3; y++)
    //        {
    //            Transform tile = grid[x, y];
    //            if(tile == null || !tile.CompareTag("TileShape"))
    //            {
    //                isSquareFull = false;
    //                break;
    //            }
    //            if (isSquareFull)
    //            {
    //                for(x = startX; x < startX + 3; x++)
    //                {
    //                    for (y = 0; y < heighty; y++)
    //                    {
    //                        if (tile != null || tile.CompareTag("TileShape"))
    //                        {
    //                            Destroy(tile.gameObject);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
        
     
    //}
    //public void HightLightTile(Tile tileShape, Transform tetromino)
    //{
    //    int x = (int)Mathf.Round(tileShape.transform.position.x);
    //    int y = (int)Mathf.Round(tileShape.transform.position.y);
    //    foreach (Transform tile in tetromino)
    //    {
    //        float distance = Vector3.Distance(tile.transform.position, grid[x,y].position);
    //        if (distance <= 0.5f)
    //        {
    //            tile.GetComponent<Tile>().HighlightTile();
    //        }
    //        else
    //        {
    //            tile.GetComponent<Tile>().ResetColor();
    //        }
    //    }
    //}
}
