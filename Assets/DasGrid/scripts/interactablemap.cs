using UnityEngine;
using System.IO;

public class interactablemap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
      grid grid = new grid(28, 16, 1.6f, Vector3.zero);  
      GetComponent<GridDrawer>().setGrid(grid);
      GetComponent<GridSpawnerinteractable>().SetGrid(grid);
      
      string path = Application.streamingAssetsPath + "/map1interactables.txt";
      string[] lines = File.ReadAllLines(path);
      for (int y = 0; y < lines.Length; y++)
{
    string[] cells = lines[y].Split(',');
    for (int x = 0; x < cells.Length; x++)
    {
        string[] cellData = cells[x].Split('-');
string entityName = cellData[0];
float rotation = float.Parse(cellData[1]);
grid.SetEntity(x, y, entityName, new Vector3(0, rotation, 0));
    }
}
      GetComponent<GridSpawnerinteractable>().SpawnEntities();
    }
    
}
