using Unity.Collections;
using UnityEngine;
using UnityEditor;

public class GridDrawer : MonoBehaviour
{
   private grid grid;
   public void setGrid (grid grid)
    {
         this.grid = grid;
    }
    private void OnDrawGizmos()
    {
        if (grid == null) return;
        for (int x = 0; x < grid.Width; x++)
{
    for (int y = 0; y < grid.Height; y++)
    {
        Vector3 Bottomleft = grid.GetWorldPosition(x, y);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(Bottomleft, Bottomleft + new Vector3(grid.CellSize, 0, 0));
        Gizmos.DrawLine(Bottomleft, Bottomleft + new Vector3(0, 0, grid.CellSize));

        Vector3 Center = Bottomleft + new Vector3(grid.CellSize * 0.25f, 0, grid.CellSize * 0.5f);
        ObjectsInGrid cell = grid.GetCell(x, y);
        Handles.Label(Center, cell.ToString());
}
    }
}
}
