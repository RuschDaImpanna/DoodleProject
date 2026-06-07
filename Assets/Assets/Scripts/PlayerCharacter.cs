using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{

    //public Cuttercontroller cuttingScript;
    //public Pastecontroller pasteScript;

    private InputSystem_Actions playerActions;
    private float speed = 3;
    private Rigidbody rb;

    public float hp = 5;
    public int score = 0;
    private Transform transformObj;


    public GameObject attackChild;
    private Transform attackPlace;
    [SerializeField] private LayerMask interactablesLayer;
    public float attackRange;
    private Vector3 attackHitbox;

    public HealthBarScript hb;
    public ScoringScript sc;

    private void Awake()
    {
        hb.setMaxHealth(hp);

        attackPlace = attackChild.transform;

        if (attackPlace.localPosition.y != 0)
        {
            
            attackHitbox = new Vector3(0.5f, 0.5f, attackRange);

        } else if (attackPlace.localPosition.x != 0)
        {
            
            attackHitbox = new Vector3(attackRange, 0.5f, 0.5f);

        }

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
            attackHitbox = new Vector3(0.5f, 0.5f, attackRange);

        }
        else if (move.y < 0)
        {

            attackPlace.localPosition = new Vector3(0, -0.65f, 0);
            attackHitbox = new Vector3(0.5f, 0.5f, attackRange);

        }
        else if (move.x != 0)
        {

            attackPlace.localPosition = new Vector3 (0.65f, 0, 0);
            attackHitbox = new Vector3(attackRange, 0.5f, 0.5f);

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

        Collider[] hits = Physics.OverlapBox(attackPlace.position, attackHitbox, Quaternion.identity, interactablesLayer);

        foreach (Collider hit in hits)
        {

            Debug.Log("Detectado: " + hit.gameObject.name);
            scoreDraw(0);

        }

    }

    private void OnDrawGizmosSelected()
    {

        if (attackChild == null) return;

        Gizmos.color = Color.red;
        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.DrawWireCube(attackChild.transform.position, attackHitbox);

    }

    private void scoreDraw(int toScore)
    {
    
        score += toScore;
        sc.updateScore(score);

    }


}
