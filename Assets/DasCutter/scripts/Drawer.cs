using UnityEngine;
using System.Collections.Generic;
public class Drawer
{
    public List<Gridchunk> chunks = new List<Gridchunk>();
    public int maxChunks = 3;

    public bool IsFull => chunks.Count >= maxChunks;

    public void addChunk(Gridchunk chunk)
    {
        if (!IsFull)
        {
            chunks.Add(chunk);
        }
    }

    public void RemoveChunk(int index)
    {
        if (index >= 0 && index < chunks.Count)
        {
            chunks.RemoveAt(index);
        }
    }
}
