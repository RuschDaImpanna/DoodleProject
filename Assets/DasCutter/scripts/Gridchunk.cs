using UnityEngine;

public class Gridchunk
{
  public int width;
  public int height;
  public ObjectsInGrid[,] structureLayer;
  public ObjectsInGrid[,] interactableLayer;

  public Gridchunk(int width, int height)
    {
        this.width = width;
        this.height = height;
        structureLayer = new ObjectsInGrid[width, height];
        interactableLayer = new ObjectsInGrid[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                structureLayer[x, y] = new ObjectsInGrid();
                interactableLayer[x, y] = new ObjectsInGrid();
            }
        }
    }

}
