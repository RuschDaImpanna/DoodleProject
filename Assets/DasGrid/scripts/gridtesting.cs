using UnityEngine;

public class gridtesting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
      grid grid = new grid(28, 16, 0.16f, Vector3.zero);  
      GetComponent<GridDrawer>().setGrid(grid);
    }
}
