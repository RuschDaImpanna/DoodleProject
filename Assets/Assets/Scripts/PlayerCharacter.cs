using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{

    //public Cuttercontroller cuttingScript;
    //public Pastecontroller pasteScript;

    private InputSystem_Actions playerActions;
    private float speed = 3;
    private Rigidbody rb;
    private Transform transformObj;

    public int hp = 5;
    public int score = 0;

    public GameObject attackChild;
    private Transform attackPlace;
    [SerializeField] private LayerMask interactablesLayer;

    private void Awake()
    {
        attackPlace = attackChild.GetComponent<Transform>();
        playerActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        transformObj = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        playerActions.Player.Attack.performed += attack;
        playerActions.Enable();
    }

    private void FixedUpdate()
    {
        //if (cuttingScript != null && pasteScript != null)
        {

            //if (!cuttingScript.isCutting || !pasteScript.isPasting)
            {

                move();

            }

        }


    }

    private void move()
    {
    
        Vector2 move = playerActions.Player.Move.ReadValue<Vector2>();

        rb.linearVelocity = new Vector3(move.x, 0, move.y) * speed;

        if (move.y > 0)
        {
            
            attackPlace.localPosition = new Vector3 (0, 0.65f, 0);

        }
        else if (move.y < 0)
        {

            attackPlace.localPosition = new Vector3(0, -0.65f, 0);

        }
        else if (move.x != 0)
        {

            attackPlace.localPosition = new Vector3 (0.65f, 0, 0);

        }

        if (move.x < 0)
        {

            float current = Mathf.Abs(transformObj.localScale.x);
            transformObj.localScale = new Vector3 (current * -1, transformObj.localScale.y, transformObj.localScale.z);

        } else if (move.x > 0)
        {

            transformObj.localScale = new Vector3 (Mathf.Abs(transformObj.localScale.x), transformObj.localScale.y, transformObj.localScale.z);

        }

    }
    private void attack(InputAction.CallbackContext context)
    {

        Debug.Log("attack");

    }


}
