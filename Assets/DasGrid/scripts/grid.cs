using UnityEngine;

public class grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private ObjectsInGrid[,] gridArray;

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public Vector3 OriginPosition => originPosition;

    public grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new ObjectsInGrid[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new ObjectsInGrid();
                
            }
        }
        Debug.Log($"Grid created — width: {width}, height: {height}, cellSize: {cellSize}");
        }    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize); // Z for 3D
    }
 
    // Grid coords → world position (center of cell)
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }
 
    private bool InBounds(int x, int y) =>
        x >= 0 && y >= 0 && x < width && y < height;
 
    public ObjectsInGrid GetCell(int x, int y)
    {
        if (!InBounds(x, y)) return null;
        return gridArray[x, y];
    }
 
    public void SetEntity(int x, int y, string entity, Vector3 rotation)
    {
        if (!InBounds(x, y)) return;
        gridArray[x, y].entity = entity;
        gridArray[x, y].rotation = rotation;
    }
 
    public void ClearCell(int x, int y)
    {
        if (!InBounds(x, y)) return;
        gridArray[x, y] = new ObjectsInGrid();
    }

}

