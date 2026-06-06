using UnityEngine;

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
                        Instantiate(ep.prefab, spawnPosition, Quaternion.Euler(cell.rotation));
                        break;
                    }
                }
            }
        }
        
    }
}
