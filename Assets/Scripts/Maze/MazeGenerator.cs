using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MazeGenerator
{
    public int Width = 1;
    public int Height = 1;
    private int _currentLevel;

    private Saves _save;

    ///////////////////////////////////////


    public void ResetAndGenerateMaze()
    {
        Width = 1;
        Height = 1;

        MazeGeneratorCell[,] cells = new MazeGeneratorCell[Width, Height];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        Maze mazeInstance = new Maze
        {
            cells = cells,
            finishPosition = Vector2Int.zero // or any default value
        };

        RemoveWallsWithBacktracker(cells); // Re-generate the maze as needed
    }


    ///////////////////////////////////////

    //Remove int level

    public Maze GenerateMaze(Cell[] CellPrefabs, int level)
    {
        _save = new Saves();

        _currentLevel = _save.GetCurrentLevel();

        _currentLevel = level;

        if (_currentLevel % 2 == 0)
        {
            Width = _currentLevel / 2 + 2;
            Height = _currentLevel / 2 + 3;
        }
        else
        {
            Width = (((_currentLevel + 1) / 2) + 1) + 1;
            Height = (((_currentLevel + 1) / 2) + 1) + 1;
        }

        MazeGeneratorCell[,] cells = new MazeGeneratorCell[Width, Height];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cells[x, y] = new MazeGeneratorCell
                {
                    X = x,
                    Y = y,
                    Visited = cells[x, y].Visited,
                    cellPrefabID = 0
                };
            }
        }

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            cells[x, Height - 1].WallLeft = false;
        }

        for (int y = 0; y < cells.GetLength(1); y++)
        {
            cells[Width - 1, y].WallBottom = false;
        }

        RemoveWallsWithBacktracker(cells);

        Maze maze = new Maze();

        maze.cells = cells;
        maze.finishPosition = PlaceMazeExit(cells);

        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < Width - 2 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < Height - 2 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);

                chosen.Visited = true;
                stack.Push(chosen);
                chosen.DistanceFromStart = current.DistanceFromStart + 1;
                current = chosen;
            }
            else
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);
    }

    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else
        {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }

    private Vector2Int PlaceMazeExit(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell furthest = maze[0, 0];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, Height - 2].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, Height - 2];
            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, 0];
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            if (maze[Width - 2, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[Width - 2, y];
            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[0, y];
        }

        if (furthest.X == 0) furthest.WallLeft = false;
        else if (furthest.Y == 0) furthest.WallBottom = false;
        else if (furthest.X == Width - 2) maze[furthest.X + 1, furthest.Y].WallLeft = false;
        else if (furthest.Y == Height - 2) maze[furthest.X, furthest.Y + 1].WallBottom = false;

        return new Vector2Int(furthest.X, furthest.Y);
    }

    // Save the maze configuration to a file
    public void SaveMazeConfig(string filePath, MazeGeneratorCell[,] maze)
    {
        MazeConfig config = new MazeConfig
        {
            Width = Width,
            Height = Height,
            CurrentLevel = _currentLevel,
            MazeStructure = new int[Width, Height],
            WallLeft = new bool[Width, Height ],
            WallBottom = new bool[Width, Height],
            FinishPosition = new Vector2Int(),
            cellPrefabID = new int[Width, Height]
        };

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                config.MazeStructure[x, y] = maze[x, y].Visited ? 1 : 0;
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                config.WallLeft[x, y] = maze[x, y].WallLeft;
                config.WallBottom[x, y] = maze[x, y].WallBottom;
                config.cellPrefabID[x, y] = maze[x, y].cellPrefabID;
            }
        }

        config.FinishPosition = PlaceMazeExit(maze);

        string jsonConfig = JsonConvert.SerializeObject(config);
        System.IO.File.WriteAllText(filePath, jsonConfig);
    }


    // Load the maze configuration from a file
    public Maze LoadMazeConfig(string filePath)
    {
        string jsonConfig = System.IO.File.ReadAllText(filePath);
        MazeConfig config = JsonConvert.DeserializeObject<MazeConfig>(jsonConfig);

        Width = config.Width;
        Height = config.Height;

        MazeGeneratorCell[,] cells = new MazeGeneratorCell[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cells[x, y] = new MazeGeneratorCell
                {
                    X = x,
                    Y = y,
                    Visited = config.MazeStructure[x, y] == 1,
                    cellPrefabID = config.cellPrefabID[x, y]  // Assign cellPrefabID

                };
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cells[x, y].WallLeft = config.WallLeft[x, y];
                cells[x, y].WallBottom = config.WallBottom[x, y];
            }
        }

        Maze maze = new Maze
        {
            cells = cells,
            finishPosition = config.FinishPosition
        };

        return maze;
    }
}