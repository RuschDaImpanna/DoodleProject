using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class GridSpawnerinteractable : MonoBehaviour
{
        [System.Serializable]
    public class EntityPrefab
    {
        public string entityName;
        public GameObject prefab;
    }
    public EntityPrefab[] entities;
    private grid grid;

    public void SetGrid(grid grid)
    {
        this.grid = grid;
    }
    public void SpawnEntities()
{
    for (int x = 0; x < grid.Width; x++)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            ObjectsInGrid cell = grid.GetCell(x, y);
            if (cell.entity == "NULL") continue;
            Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize * 0.5f, 0.1f, grid.CellSize * 0.5f);

            foreach (EntityPrefab ep in entities)
            {
                Debug.Log($"cell entity: '{cell.entity}' — ep.entityName: '{ep.entityName}'");
                if (cell.entity.StartsWith(ep.entityName))
                {
                    GameObject spawned = Instantiate(ep.prefab, spawnPosition, Quaternion.Euler(cell.rotation));
                    Debug.Log($"Spawned: {spawned.name}, ChestData: {spawned.GetComponent<ChestData>()}");
                    if (cell.entity.StartsWith("chest"))
                    {
                        ChestData chestData = spawned.GetComponent<ChestData>();
                        FindChestData(cell.entity, chestData);
                    }
                    break;
                }
            }
        }
    }
}

private void FindChestData(string chestName, ChestData chestData)
{
    string path = Application.streamingAssetsPath + "/chestdata.txt";
    string[] lines = File.ReadAllLines(path);

    foreach (string line in lines)
    {
        string[] data = line.Split(',');
        if (data[0] == chestName)
        {
            bool isOpen = data[1] == "open";
            List<string> items = new List<string>(data[2].Split('-'));
            chestData.Initialize(chestName, isOpen, items);
            chestData.Initialize(chestName, isOpen, items);
Debug.Log($"Initialized {chestName} — open: {isOpen}, items: {string.Join(", ", items)}");
            return;
            
        }
    }
    
}
}
