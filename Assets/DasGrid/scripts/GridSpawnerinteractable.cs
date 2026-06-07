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
    private List<GameObject> spawnedObjects = new List<GameObject>();

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
                        spawnedObjects.Add(spawned);
                        Debug.Log($"Spawned: {spawned.name}, ChestData: {spawned.GetComponent<ChestData>()}");
                        if (cell.entity.StartsWith("chest"))
                        {
                            ChestData chestData = spawned.GetComponent<ChestData>();
                            FindChestData(cell.entity, chestData);
                        }
                        if (cell.entity.StartsWith("door"))
                        {
                            DoorData doorData = spawned.GetComponent<DoorData>();
                            FindDoorData(cell.entity, doorData);
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
                Debug.Log($"Initialized {chestName} — open: {isOpen}, items: {string.Join(", ", items)}");
                return;
            }
        }
    }

    public void SpawnSingleEntity(int x, int y, string entityName, Vector3 rotation)
    {
        if (entityName == "NULL") return;

        Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize * 0.5f, 0.1f, grid.CellSize * 0.5f);

        foreach (EntityPrefab ep in entities)
        {
            if (entityName.StartsWith(ep.entityName))
            {
                GameObject spawned = Instantiate(ep.prefab, spawnPosition, Quaternion.Euler(rotation));
                spawnedObjects.Add(spawned);

                if (entityName.StartsWith("chest"))
                {
                    ChestData chestData = spawned.GetComponent<ChestData>();
                    FindChestData(entityName, chestData);
                }
                if (entityName.StartsWith("door"))
                {
                    DoorData doorData = spawned.GetComponent<DoorData>();
                    FindDoorData(entityName, doorData);
                }
                break;
            }
        }
    }

    private void FindDoorData(string doorID, DoorData doorData)
    {
        string path = Application.streamingAssetsPath + "/doors.txt";
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            string[] data = line.Split(',');
            if (data[0] == doorID)
            {
                doorData.Initialize(doorID, data[1], data[2]);
                Debug.Log($"Initialized {doorID} → {data[1]} at {data[2]}");
                return;
            }
        }
    }

    public void ClearSpawned()
    {
        foreach (GameObject obj in spawnedObjects)
            Destroy(obj);
        spawnedObjects.Clear();
    }
}