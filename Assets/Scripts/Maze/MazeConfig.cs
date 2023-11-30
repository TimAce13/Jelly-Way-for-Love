using System;
using UnityEngine;

[Serializable]
public class MazeConfig
{
    public int Width;
    public int Height;
    public int CurrentLevel;
    public int[,] MazeStructure;
    public bool[,] WallLeft;
    public bool[,] WallBottom;
    public Vector2Int FinishPosition;
    public int[,] cellPrefabID;
}