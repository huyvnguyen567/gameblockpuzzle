using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Transform gridContainer; // Container chứa các ô trong lưới grid
    private Transform spawnPoints;
    [SerializeField] private ObjectPooling pool;
    [SerializeField] private GameObject gridContainPrefab;
    [SerializeField] private GameObject spawnPointsPrefab;
    [SerializeField] private GameObject tilePrefab; // Prefab của ô (tile)
    [SerializeField] private int width = 9;
    [SerializeField] private int height = 9;
    [SerializeField] private List<GameObject> tetrominoPrefabs;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Vector3 scaleTile = new Vector3(0.3f, 0.3f, 1);
    [SerializeField] private float scaleTileTimer = 0.3f;
    [SerializeField] private float timeDestroyTile = 0.3f;

    private List<GameObject> currentTetrominos = new List<GameObject>();
    private List<Tile> listTiles = new List<Tile>();
    public Transform[,] grid; // Lưới grid lưu trữ thông tin về ô
    private bool gameOver = false;
    [SerializeField] private bool checkDestroyed = false;
    public bool CheckDestroyed
    {
        get { return checkDestroyed; }
        set
        {
            checkDestroyed = value;
        }
    }
    public bool GameOver
    {
        get { return gameOver; }
        set
        {
            gameOver = value;
        }
    }

    public int Width => width;
    public int Height => height;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnGridContainAndSpawnPoints();
        // Khởi tạo lưới grid và các thông tin liên quan
        InitializeGrid();
        // Sinh ngẫu nhiên các khối tetromino
        SpawnInitialTetrominos();
        DataManager.Instance.Score = 0;
        UIController.Instance.ShowWindow(WindowType.Mainmenu, false);
        UIController.Instance.UpdateWindow(WindowType.Gameplay);
        UIController.Instance.ShowWindow(WindowType.Gameplay, true);
    }

    private void Update()
    {
       
    }
    private void SpawnGridContainAndSpawnPoints()
    {
        gridContainer = Instantiate(gridContainPrefab.transform);
        spawnPoints = Instantiate(spawnPointsPrefab.transform);
     

    }
    public void IncreaseScore(int amount)
    {
        DataManager.Instance.Score += amount;
        if(DataManager.Instance.Score > DataManager.Instance.HighScore)
        {
            DataManager.Instance.HighScore = DataManager.Instance.Score;
            PlayerPrefs.SetInt("HighScore", DataManager.Instance.HighScore);
        }
        UIController.Instance.UpdateWindow(WindowType.Gameplay);
    }
    private void InitializeGrid()
    {
        grid = new Transform[width, height]; // Khởi tạo lưới grid có kích thước width * height

        // Lặp qua tất cả các ô trong lưới grid để khởi tạo các ô
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Tạo GameObject từ prefab của ô (tile)
                //GameObject newTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                GameObject newTile = pool.GetObjectFromPool().gameObject;
                newTile.transform.position = new Vector3(x, y, 0);

                newTile.name = $"Tile {x} {y}";
                newTile.transform.SetParent(gridContainer);
                listTiles.Add(newTile.GetComponent<Tile>());

                // Lưu trữ transform của ô trong lưới grid
                grid[x, y] = newTile.transform;
            }
        }
        ChangeColorTileGrid(0, 3);
        ChangeColorTileGrid(3, 0);
        ChangeColorTileGrid(6, 3);
        ChangeColorTileGrid(3, 6);

        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 2f, -10);
    }
    public void ChangeColorTileGrid(int startX, int startY)
    {
        for (int x = startX; x < startX + 3; x++)
        {
            for (int y = startY; y < startY + 3; y++)
            {
                Transform tile = grid[x, y];
                Color currentColor = tile.GetComponent<SpriteRenderer>().color;
                currentColor.a = 0.8f;
                tile.GetComponent<SpriteRenderer>().color = currentColor;
            }
        }
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
        foreach (Transform point in spawnPoints)
        {
            GameObject tetrominoPrefab = GetRandomTetrominoPrefab();
            Transform spawnPosition = point;
            GameObject tetromino = Instantiate(tetrominoPrefab, spawnPosition.position, Quaternion.identity);
            tetromino.transform.SetParent(spawnPosition);
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
                float offset = 0;
                //Debug.Log("Full dòng " +y);
                //Xóa dòng
                for (int x = 0; x < width; x++)
                {
                    offset += 0.03f;
                    Transform tile = grid[x, y];
                    if (tile != null && tile.CompareTag("TileShape"))
                    {
                        IncreaseScore(DataManager.Instance.ScorePerBlock);
                        DataManager.Instance.ScoreAmount += DataManager.Instance.ScorePerBlock;
                        tile.DOScale(scaleTile + new Vector3(offset, offset, 0), scaleTileTimer);
                        tile.DORotate(tile.localRotation.eulerAngles + new Vector3(0, 0, -180), scaleTileTimer);
                        Destroy(tile.gameObject, timeDestroyTile);    
                    }
                }
                checkDestroyed = true;

            }
        }
    }
    public void CheckAndClearFullColumns()
    {
        for (int x = 0; x < width; x++)
        {
            bool isColumnFull = true;
            for (int y = 0; y < height; y++)
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
                float offset = 0;

                //Debug.Log("Full cột " + x);

                //Xóa cột
                for (int y = 0; y < height; y++)
                {
                    offset += 0.03f;
                    Transform tile = grid[x, y];
                    if (tile != null && tile.CompareTag("TileShape"))
                    {
                        IncreaseScore(DataManager.Instance.ScorePerBlock);
                        DataManager.Instance.ScoreAmount += DataManager.Instance.ScorePerBlock;
                        tile.DOScale(scaleTile + new Vector3(offset, offset, 0), scaleTileTimer);
                        tile.DORotate(tile.localRotation.eulerAngles + new Vector3(0, 0, -180), scaleTileTimer);
                        Destroy(tile.gameObject, timeDestroyTile);
                    }
                }
                checkDestroyed = true;

            }
        }
    }
    public void CheckAndClearSquare(int startX, int startY)
    {
        bool isSquareFull = true;

        for (int x = startX; x < startX + 3; x++)
        {
            for (int y = startY; y < startY + 3; y++)
            {
                Transform tile = grid[x, y];
                if (tile == null || !tile.CompareTag("TileShape"))
                {
                    isSquareFull = false;
                    break;
                }
            }
        }
        if (isSquareFull)
        {
            float offset = 0;
            for (int x = startX; x < startX + 3; x++)
            {
                for (int y = startY; y < startY + 3; y++)
                {
                    offset += 0.03f;
                    Transform tile = grid[x, y];
                    if (tile != null || tile.CompareTag("TileShape"))
                    {
                        IncreaseScore(DataManager.Instance.ScorePerBlock);
                        DataManager.Instance.ScoreAmount += DataManager.Instance.ScorePerBlock;
                        tile.DOScale(scaleTile + new Vector3(offset,offset,0), scaleTileTimer);
                        tile.DORotate(tile.localRotation.eulerAngles + new Vector3(0, 0, -180), scaleTileTimer);
                        Destroy(tile.gameObject, timeDestroyTile);
                    }
                }
            }
            checkDestroyed = true;

        }
    }
    public void CheckGameOver()
    {
        bool canAdd = false;
        
        foreach (GameObject tetromino in currentTetrominos)
        {
            if(tetromino != null)
            {
                for (int i = 0; i < width + 1 - tetromino.GetComponent<Tetromino>().Width; i++)
                {
                    for (int j = 0; j < height + 1 - tetromino.GetComponent<Tetromino>().Height; j++)
                    {
                        bool isEmpty = true;
                        foreach (Transform tile in tetromino.transform)
                        {
                            int x = (int)Mathf.Round(tile.localPosition.x) + i;
                            int y = (int)Mathf.Round(tile.localPosition.y) + j;

                            if ((x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1)))
                            {
                                Transform gridCell = grid[x, y];
                                //Debug.Log(x + " " + y + " " + tetromino.name);
                                if (gridCell != null && gridCell.CompareTag("TileShape"))
                                {
                                    //Debug.Log("x: " + x + " y: " + y + /*" name: " +gridCell.name +*/ "tag: " + gridCell.CompareTag("TileShape"));
                                    isEmpty = false;
                                    break;
                                }
                            }
                        }
                        if (isEmpty)
                        {
                            canAdd = true;
                            break;
                        }
                    }
                }
            }    
        }
        if (canAdd)
        {
            //Debug.Log("Tiếp tục chơi!");
        }
        else
        {
            gameOver = true;
            UIController.Instance.UpdatePopup(PopupType.Gameover);
            UIController.Instance.ShowPopup(PopupType.Gameover, true);

            Debug.Log("Thua! Không thể đặt tetromino nào vào grid.");
        }

    }
    public void ReplayGame()
    {
        SceneManager.LoadScene("Game Play");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
   
}



