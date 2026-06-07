using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionBox : MonoBehaviour
{
    private InputSystem_Actions playerActions;
    private LineRenderer lineRenderer;

    private int currentX;
    private int currentY;
    private int width = 2;
    private int height = 3;
    private bool isActive = false;

    private grid structureGrid;

    private void Awake()
    {
        playerActions = new InputSystem_Actions();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}