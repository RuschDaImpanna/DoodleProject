using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{

    private InputSystem_Actions playerActions;
    private float speed = 3;
    private Rigidbody rb;

    private void Awake()
    {
        playerActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void FixedUpdate()
    {
        Vector2 move = playerActions.Player.Move.ReadValue<Vector2>();
        Debug.Log(move);

        rb.linearVelocity = new Vector2(move.x * speed, move.y * speed);
    }

}
