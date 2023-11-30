using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public Cell CellPrefab;
    public GameObject StartFloor;
    public GameObject FinishFloor;
    public GameObject JellyPoint;
    public Vector3 CellSize = new Vector3(1,1,0);
    public HintRenderer HintRenderer;

    public Maze maze;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();

        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);

                if (x == maze.cells.GetLength(0)-1)
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
        Instantiate(FinishFloor, new Vector3(maze.finishPosition.x * CellSize.x + CellSize.x / 2, 0,maze.finishPosition.y * CellSize.z + CellSize.z/2), Quaternion.identity);

        HintRenderer.DrawPath();

        for (int x = 0; x < maze.cells.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1) - 1; y++)
            {
                var isOnTheWay = OnTheWay(maze.cells[x, y].X, maze.cells[x, y].Y);

                if (isOnTheWay == false)
                {
                    var rand = Random.Range(1, 100);
                    if (rand >= 30)
                    {
                        Instantiate(JellyPoint, new Vector3(x * CellSize.x + CellSize.x / 2, 1, y * CellSize.z + CellSize.z / 2), Quaternion.identity);
                    }
                }
            }
        }
    }

    bool OnTheWay(int x, int y)
    {
        var test = false;
        foreach (var point in HintRenderer.positions)
        {
            if (x * 10 == point.x && y * 10 == point.z)
            {
                test = true;
            }
        }

        return test;
    }
}