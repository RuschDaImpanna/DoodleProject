using UnityEngine;
using UnityEngine.InputSystem;

public class CutterController : MonoBehaviour
{
    private InputSystem_Actions playerActions;

    public LineRenderer lineRenderer;
    public GridCutter gridCutter;

    public bool isCutting = false;

    private int currentX;
    private int currentY;
    private int width = 2;
    private int height = 3;
    private float moveDelay = 0.15f;
    private float moveTimer = 0f;

    private void Awake()
    {
        playerActions = new InputSystem_Actions();
        lineRenderer.enabled = false;
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
        if (playerActions.Player.Cut.WasPressedThisFrame())
        {
            if (!isCutting)
            {
                isCutting = true;
                lineRenderer.enabled = true;
            }
            else
            {
                gridCutter.Cut(currentX, currentY, width, height);
                isCutting = false;
                lineRenderer.enabled = false;
            }
        }

        if (isCutting)
        {
            HandleMovement();
            DrawSelectionBox();
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

        currentX = Mathf.Clamp(currentX, 0, structuremap.structureGrid.Width - width);
        currentY = Mathf.Clamp(currentY, 0, structuremap.structureGrid.Height - height);
    }

    private void DrawSelectionBox()
    {
        float yOffset = 0.15f;

        Vector3 bottomLeft = structuremap.structureGrid.GetWorldPosition(currentX, currentY) + Vector3.up * yOffset;
        Vector3 bottomRight = structuremap.structureGrid.GetWorldPosition(currentX + width, currentY) + Vector3.up * yOffset;
        Vector3 topLeft = structuremap.structureGrid.GetWorldPosition(currentX, currentY + height) + Vector3.up * yOffset;
        Vector3 topRight = structuremap.structureGrid.GetWorldPosition(currentX + width, currentY + height) + Vector3.up * yOffset;

        lineRenderer.positionCount = 5;
        lineRenderer.SetPosition(0, bottomLeft);
        lineRenderer.SetPosition(1, bottomRight);
        lineRenderer.SetPosition(2, topRight);
        lineRenderer.SetPosition(3, topLeft);
        lineRenderer.SetPosition(4, bottomLeft);
    }
}