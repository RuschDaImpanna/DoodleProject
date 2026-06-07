using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public class GridCutter : MonoBehaviour
{
    public GridSpawner structureSpawner;
    public GridSpawnerinteractable interactableSpawner;
    private grid structureGrid;
    private grid interactableGrid;
    private Drawer drawer = new Drawer();

    private void Start()
    {
        SetGrids(structuremap.structureGrid, interactablemap.interactableGrid);
    }

    public void SetGrids(grid structure, grid interactable)
    {
        this.structureGrid = structure;
        this.interactableGrid = interactable;
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        string structPath = Application.streamingAssetsPath + "/clipboard_structure.txt";
        string interactPath = Application.streamingAssetsPath + "/clipboard_interactable.txt";

        if (File.Exists(structPath)) File.Delete(structPath);
        if (File.Exists(interactPath)) File.Delete(interactPath);
    }

    public void Cut(int originX, int originY, int width, int height)
    {
        if (drawer.IsFull)
        {
            Debug.Log("drawer is full");
            return;
        }

        Gridchunk chunk = new Gridchunk(width, height);
        int structureMask = LayerMask.GetMask("Structure");
        int interactableMask = LayerMask.GetMask("Interactable");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellCenter = structureGrid.GetWorldPosition(originX + x, originY + y)
                    + new Vector3(structureGrid.CellSize * 0.5f, 0.5f, structureGrid.CellSize * 0.5f);
                Vector3 halfExtents = new Vector3(structureGrid.CellSize * 0.4f, 0.5f, structureGrid.CellSize * 0.4f);

                Collider[] structHits = Physics.OverlapBox(cellCenter, halfExtents, Quaternion.identity, structureMask);
                foreach (Collider hit in structHits)
                {
                    chunk.structureLayer[x, y].entity = hit.tag;
                    chunk.structureLayer[x, y].rotation = hit.transform.eulerAngles;
                    Destroy(hit.gameObject);
                }

                Collider[] interactHits = Physics.OverlapBox(cellCenter, halfExtents, Quaternion.identity, interactableMask);
                foreach (Collider hit in interactHits)
                {
                    chunk.interactableLayer[x, y].entity = hit.tag;
                    chunk.interactableLayer[x, y].rotation = hit.transform.eulerAngles;
                    Destroy(hit.gameObject);
                }

                Debug.Log($"scanning cell {originX + x},{originY + y} — struct hits: {structHits.Length}, interact hits: {interactHits.Length}");
                structureGrid.ClearCell(originX + x, originY + y);
                interactableGrid.ClearCell(originX + x, originY + y);
            }
        }

        WriteChunkToFile(chunk);
        drawer.addChunk(chunk);
        Debug.Log($"cut saved, drawer has {drawer.chunks.Count} chunks");
    }

    private void WriteChunkToFile(Gridchunk chunk)
    {
        System.Text.StringBuilder structureSB = new System.Text.StringBuilder();
        System.Text.StringBuilder interactableSB = new System.Text.StringBuilder();

        for (int y = 0; y < chunk.height; y++)
        {
            for (int x = 0; x < chunk.width; x++)
            {
                string sep = x < chunk.width - 1 ? "," : "";
                string structEntity = string.IsNullOrEmpty(chunk.structureLayer[x, y].entity) ? "NULL" : chunk.structureLayer[x, y].entity;
                string interactEntity = string.IsNullOrEmpty(chunk.interactableLayer[x, y].entity) ? "NULL" : chunk.interactableLayer[x, y].entity;

                structureSB.Append($"{structEntity}-{chunk.structureLayer[x, y].rotation.y:F0}{sep}");
                interactableSB.Append($"{interactEntity}-{chunk.interactableLayer[x, y].rotation.y:F0}{sep}");
            }
            structureSB.AppendLine();
            interactableSB.AppendLine();
        }

        File.WriteAllText(Application.streamingAssetsPath + "/clipboard_structure.txt", structureSB.ToString());
        File.WriteAllText(Application.streamingAssetsPath + "/clipboard_interactable.txt", interactableSB.ToString());
        Debug.Log("chunk written to clipboard files");
    }

    public void Paste(int pasteX, int pasteY, int chunkIndex, GridSpawner structureSpawner, GridSpawnerinteractable interactableSpawner)
    {
        if (chunkIndex < 0 || chunkIndex >= drawer.chunks.Count)
        {
            Debug.Log("invalid chunk index");
            return;
        }

        Gridchunk chunk = drawer.chunks[chunkIndex];

        string structPath = Application.streamingAssetsPath + "/clipboard_structure.txt";
        string interactPath = Application.streamingAssetsPath + "/clipboard_interactable.txt";

        string[] structLines = File.ReadAllLines(structPath);
        string[] interactLines = File.ReadAllLines(interactPath);
        Debug.Log($"struct lines: {structLines.Length}, interact lines: {interactLines.Length}");

        for (int y = 0; y < structLines.Length; y++)
        {
            if (string.IsNullOrWhiteSpace(structLines[y])) continue;
            string[] structCells = structLines[y].Split(',');
            string[] interactCells = interactLines[y].Split(',');

            for (int x = 0; x < structCells.Length; x++)
            {
                Debug.Log($"structCell: '{structCells[x]}' interactCell: '{interactCells[x]}'");

                // destroy whatever is already at the paste destination
                Vector3 cellCenter = structureGrid.GetWorldPosition(pasteX + x, pasteY + y)
                    + new Vector3(structureGrid.CellSize * 0.5f, 0.5f, structureGrid.CellSize * 0.5f);
                Vector3 halfExtents = new Vector3(structureGrid.CellSize * 0.4f, 0.5f, structureGrid.CellSize * 0.4f);

                Collider[] hits = Physics.OverlapBox(cellCenter, halfExtents, Quaternion.identity);
                foreach (Collider hit in hits)
                    Destroy(hit.gameObject);

                // paste structure
                string[] structData = structCells[x].Split('-');
                Debug.Log($"structData length: {structData.Length}, value: '{structCells[x]}'");
                string structEntity = structData[0];
                float structRot = float.Parse(structData[1]);
                structureGrid.SetEntity(pasteX + x, pasteY + y, structEntity, new Vector3(0, structRot, 0));
                structureSpawner.SpawnSingleEntity(pasteX + x, pasteY + y, structEntity, new Vector3(0, structRot, 0));

                // paste interactable
                string[] interactData = interactCells[x].Split('-');
                Debug.Log($"interactData length: {interactData.Length}, value: '{interactCells[x]}'");
                string interactEntity = interactData[0];
                float interactRot = float.Parse(interactData[1]);
                interactableGrid.SetEntity(pasteX + x, pasteY + y, interactEntity, new Vector3(0, interactRot, 0));
                interactableSpawner.SpawnSingleEntity(pasteX + x, pasteY + y, interactEntity, new Vector3(0, interactRot, 0));
            }
        }

        drawer.RemoveChunk(chunkIndex);
        Debug.Log($"pasted chunk, drawer has {drawer.chunks.Count} chunks remaining");
    }
}