using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
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
        SpawnEntities();
    }

    public void SpawnEntities()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                ObjectsInGrid cell = grid.GetCell(x, y);
                if (cell.entity == "NULL") continue;
                Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize * 0.5f, 0, grid.CellSize * 0.5f);

                foreach (EntityPrefab ep in entities)
                {
                    if (ep.entityName == cell.entity)
                    {
                        GameObject spawned = Instantiate(ep.prefab, spawnPosition, Quaternion.Euler(cell.rotation));
                        spawnedObjects.Add(spawned);
                        break;
                    }
                }
            }
        }
    }

    public void SpawnSingleEntity(int x, int y, string entityName, Vector3 rotation)
    {
        if (entityName == "NULL") return;
        Debug.Log($"trying to spawn: '{entityName}'");
        Vector3 spawnPosition = grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize * 0.5f, 0, grid.CellSize * 0.5f);

        foreach (EntityPrefab ep in entities)
        {
            if (ep.entityName == entityName)
            {
                GameObject spawned = Instantiate(ep.prefab, spawnPosition, Quaternion.Euler(rotation));
                spawnedObjects.Add(spawned);
                break;
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