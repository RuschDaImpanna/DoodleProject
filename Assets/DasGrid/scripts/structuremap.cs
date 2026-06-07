using UnityEngine;
using System.IO;

public class structuremap : MonoBehaviour
{
    public static grid structureGrid;

    private void Start()
    {
        structureGrid = new grid(28, 16, 1.6f, Vector3.zero);
        GetComponent<GridDrawer>().setGrid(structureGrid);
        GetComponent<GridSpawner>().SetGrid(structureGrid);

        string path = Application.streamingAssetsPath + "/map1Structures.txt";
        string[] lines = File.ReadAllLines(path);
        for (int y = 0; y < lines.Length; y++)
        {
            string[] cells = lines[y].Split(',');
            for (int x = 0; x < cells.Length; x++)
            {
                string[] cellData = cells[x].Split('-');
                string entityName = cellData[0];
                float rotation = float.Parse(cellData[1]);
                structureGrid.SetEntity(x, y, entityName, new Vector3(0, rotation, 0));
            }
        }
        GetComponent<GridSpawner>().SpawnEntities();
    }
}
