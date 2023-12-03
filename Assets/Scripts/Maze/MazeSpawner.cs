using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeSpawner : MonoBehaviour
{
    public Cell[] CellPrefabs;
    public GameObject StartFloor;
    public GameObject FinishFloor;
    public Vector3 CellSize = new Vector3(1, 1, 0);
    public HintRenderer HintRenderer;

    private MazeGenerator generator;
    public Maze maze;

    private Saves _save;

    string filePath;

    ///////////////////////////////////////

    public TMP_InputField LevelField;

    public void SaveConfigClicked()
    {
        filePath = $"Assets/Scenes/LevelConfigs/MazeConfig_Lvl{LevelField.text}.json";
        generator.SaveMazeConfig(filePath, maze.cells);
    }

    public void GenerateNewMazeClicked()
    {
        Cell[] existingCells = FindObjectsOfType<Cell>();
        foreach (var cell in existingCells)
        {
            Destroy(cell.gameObject);
        }

       // maze = generator.GenerateMaze(CellPrefabs,int.Parse(LevelField.text));

        SpawnMaze();
    }

    public void GenerateExistingMazeClicked()
    {
        Cell[] existingCells = FindObjectsOfType<Cell>();
        foreach (var cell in existingCells)
        {
            Destroy(cell.gameObject);
        }

        filePath = $"Assets/Scenes/LevelConfigs/MazeConfig_Lvl{LevelField.text}.json";
        maze = generator.LoadMazeConfig(filePath);

        SpawnMaze();
    }

    ///////////////////////////////////////


    private void Start()
    {
        generator = new MazeGenerator();
        _save = new Saves();

        string filePath = $"Assets/Scenes/LevelConfigs/MazeConfig_Lvl{_save.GetCurrentLevel().ToString()}.json";

        // Uncomment the line below to generate a new maze and save its configuration
        //maze = generator.GenerateMaze(CellPrefabs);
        //generator.SaveMazeConfig(filePath, maze.cells);

        //Uncomment the line below to load the maze configuration and spawn the maze
        maze = generator.LoadMazeConfig(filePath);

        SpawnMaze();
    }

    private void SpawnMaze()
    {
        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                var cellType = maze.cells[x, y].cellPrefabID;

                Cell c = Instantiate(CellPrefabs[cellType], new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);

                if (x == maze.cells.GetLength(0) - 1)
                {
                    c.Floor.SetActive(false);
                }
                if (y == maze.cells.GetLength(1) - 1)
                {
                    c.Floor.SetActive(false);
                }
            }
        }

        Instantiate(StartFloor, new Vector3(CellSize.x / 2, 0, CellSize.z / 2), Quaternion.identity);
        Instantiate(FinishFloor, new Vector3(maze.finishPosition.x * CellSize.x + CellSize.x / 2, 0, maze.finishPosition.y * CellSize.z + CellSize.z / 2), Quaternion.identity);

        HintRenderer.DrawPath();
    }
}