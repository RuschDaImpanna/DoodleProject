using UnityEngine;
using System.IO;

public class interactablemap : MonoBehaviour
{
    public static grid interactableGrid;

    private void Start()
    {
        interactableGrid = new grid(28, 16, 1.6f, Vector3.zero);
        GetComponent<GridDrawer>().setGrid(interactableGrid);

        string path = Application.streamingAssetsPath + "/R00_interactables.txt";
        string[] lines = File.ReadAllLines(path);
        for (int y = 0; y < lines.Length; y++)
        {
            string[] cells = lines[y].Split(',');
            for (int x = 0; x < cells.Length; x++)
            {
                string[] cellData = cells[x].Split('-');
                string entityName = cellData[0];
                float rotation = float.Parse(cellData[1]);
                interactableGrid.SetEntity(x, y, entityName, new Vector3(0, rotation, 0));
            }
        }
        GetComponent<GridSpawnerinteractable>().SetGrid(interactableGrid);
        GetComponent<GridSpawnerinteractable>().SpawnEntities();
    }
    public void ClearRoom()
{
    GetComponent<GridSpawnerinteractable>().ClearSpawned();

    for (int x = 0; x < interactableGrid.Width; x++)
        for (int y = 0; y < interactableGrid.Height; y++)
            interactableGrid.ClearCell(x, y);
}
public void LoadRoom(string roomName)
{
    string path = Application.streamingAssetsPath + "/" + roomName + "_interactables.txt";
    string[] lines = File.ReadAllLines(path);
    for (int y = 0; y < lines.Length; y++)
    {
        string[] cells = lines[y].Split(',');
        for (int x = 0; x < cells.Length; x++)
        {
            string[] cellData = cells[x].Split('-');
            string entityName = cellData[0];
            float rotation = float.Parse(cellData[1]);
            interactableGrid.SetEntity(x, y, entityName, new Vector3(0, rotation, 0));
        }
    }
    GetComponent<GridSpawnerinteractable>().SpawnEntities();
}
}