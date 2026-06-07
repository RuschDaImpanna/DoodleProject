using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections.Generic;

public class PasterController : MonoBehaviour
{
    private InputSystem_Actions playerActions;

    public LineRenderer lineRenderer;
    public GridCutter gridCutter;
    public GridSpawner structureSpawner;
    public GridSpawnerinteractable interactableSpawner;

    public bool isPasting = false;

    private int currentX;
    private int currentY;
    private float moveDelay = 0.15f;
    private float moveTimer = 0f;

    private int chunkWidth;
    private int chunkHeight;
    [System.Serializable]
    public class EntityPrefab
    {
        public string entityName;
        public GameObject prefab;
    }

    public EntityPrefab[] structureEntities;
    public EntityPrefab[] interactableEntities;

    private List<GameObject> ghostObjects = new List<GameObject>();

    private void Awake()
    {
        playerActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Update()
    {
        if (playerActions.Player.Paste.WasPressedThisFrame())
        {
            if (!isPasting)
            {
                isPasting = true;
                SpawnGhosts();
            }
            else
            {
                gridCutter.Paste(currentX, currentY, 0, structureSpawner, interactableSpawner);
                ClearGhosts();
                isPasting = false;
            }
        }

        if (isPasting)
        {
            HandleMovement();
            UpdateGhostPositions();
        }
    }

    private void HandleMovement()
{
    moveTimer -= Time.deltaTime;
    if (moveTimer > 0) return;

    Vector2 move = playerActions.Player.Move.ReadValue<Vector2>();

    if (move.y > 0.5f) { currentY++; moveTimer = moveDelay; }
    else if (move.y < -0.5f) { currentY--; moveTimer = moveDelay; }
    else if (move.x > 0.5f) { currentX++; moveTimer = moveDelay; }
    else if (move.x < -0.5f) { currentX--; moveTimer = moveDelay; }

    currentX = Mathf.Clamp(currentX, 0, structuremap.structureGrid.Width - chunkWidth);
    currentY = Mathf.Clamp(currentY, 0, structuremap.structureGrid.Height - chunkHeight);
}

    private void SpawnGhosts()
{
    Debug.Log("SpawnGhosts called");
    ClearGhosts();

    string structPath = Application.streamingAssetsPath + "/clipboard_structure.txt";
    string interactPath = Application.streamingAssetsPath + "/clipboard_interactable.txt";

    if (!File.Exists(structPath) || !File.Exists(interactPath))
    {
        Debug.Log("no clipboard data found");
        isPasting = false;
        return;
    }

    string[] structLines = File.ReadAllLines(structPath);
    string[] interactLines = File.ReadAllLines(interactPath);

    chunkHeight = 0;
    chunkWidth = 0;

    for (int y = 0; y < structLines.Length; y++)
    {
        if (string.IsNullOrWhiteSpace(structLines[y])) continue;
        string[] structCells = structLines[y].Split(',');
        string[] interactCells = interactLines[y].Split(',');

        chunkWidth = structCells.Length;
        chunkHeight++;

        for (int x = 0; x < structCells.Length; x++)
        {
            SpawnGhostEntity(structCells[x], x, y, structureEntities);
            SpawnGhostEntity(interactCells[x], x, y, interactableEntities);
        }
    }
}

private void SpawnGhostEntity(string cellData, int x, int y, EntityPrefab[] entityList)
{
    string[] data = cellData.Split('-');
    string entityName = data[0];
    Debug.Log($"trying to spawn ghost: '{entityName}'");

    if (entityName == "NULL") return;

    foreach (EntityPrefab ep in entityList)
    {
        if (entityName.StartsWith(ep.entityName))
        {
            Vector3 spawnPos = structuremap.structureGrid.GetWorldPosition(currentX + x, currentY + y)
                + new Vector3(structuremap.structureGrid.CellSize * 0.5f, 0.15f, structuremap.structureGrid.CellSize * 0.5f);
            GameObject ghost = Instantiate(ep.prefab, spawnPos, Quaternion.Euler(0, float.Parse(data[1]), 0));
            ghostObjects.Add(ghost);
            break;
        }
    }
}

    private void UpdateGhostPositions()
{
    SpawnGhosts();
}

    private void ClearGhosts()
    {
        foreach (GameObject g in ghostObjects)
            Destroy(g);
        ghostObjects.Clear();
    }
}