using UnityEngine;

public class BeeMove : MonoBehaviour
{
    
    public enum State
    {
        Hover,
        Fly,
        Descend,
        Grounded,
        Takeoff
    }

    public State currentState;

    [Header("Movement")]
    public float speed = 3f;
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 2f;

    [Header("Heights")]
    public float minHeight = 1f;
    public float maxHeight = 5f;
    public float descendSpeed = 2f;
    public float takeoffSpeed = 3f;

    [Header("Area Bounds")]
    public Vector3 areaCenter;
    public Vector3 areaSize = new Vector3(10, 5, 10);

    [Header("Timers")]
    public float minFlyTime = 2f;
    public float maxFlyTime = 5f;

    public float minGroundTime = 2f;
    public float maxGroundTime = 4f;

    private float stateTimer;
    private Vector3 targetPosition;
    private float baseY;

    void Start()
    {
        currentState = State.Hover;
        baseY = transform.position.y;
        SetRandomTarget();
        ResetTimer(minFlyTime, maxFlyTime);
    }

    void Update()
    {
        HandleBounds();

        switch (currentState)
        {
            case State.Hover:
                Hover();
                if (stateTimer <= 0)
                {
                    currentState = State.Fly;
                    ResetTimer(minFlyTime, maxFlyTime);
                }
                break;

            case State.Fly:
                Fly();
                if (stateTimer <= 0)
                {
                    currentState = State.Descend;
                }
                break;

            case State.Descend:
                Descend();
                break;

            case State.Grounded:
                Grounded();
                break;

            case State.Takeoff:
                Takeoff();
                break;
        }

        stateTimer -= Time.deltaTime;
    }

    // 🪰 HOVER
    void Hover()
    {
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        Vector3 pos = transform.position;
        pos.y = baseY + hoverOffset;
        transform.position = pos;
    }

    // ✈️ FLY
    void Fly()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        LookAtDirection(direction);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetRandomTarget();
        }
    }

    // 🛬 DESCEND
    void Descend()
    {
        Vector3 pos = transform.position;
        pos.y -= descendSpeed * Time.deltaTime;
        transform.position = pos;

        if (pos.y <= minHeight)
        {
            pos.y = minHeight;
            transform.position = pos;

            currentState = State.Grounded;
            ResetTimer(minGroundTime, maxGroundTime);
        }
    }

    // 🐝 GROUNDED
    void Grounded()
    {
        if (stateTimer <= 0)
        {
            currentState = State.Takeoff;
        }
    }

    // 🚀 TAKEOFF
    void Takeoff()
    {
        Vector3 pos = transform.position;
        pos.y += takeoffSpeed * Time.deltaTime;
        transform.position = pos;

        if (pos.y >= baseY)
        {
            currentState = State.Hover;
            ResetTimer(minFlyTime, maxFlyTime);
        }
    }

    // 🎯 RANDOM TARGET
    void SetRandomTarget()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
            Random.Range(minHeight, maxHeight),
            Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
        );

        targetPosition = randomPoint;
    }

    // 📦 LIMIT AREA
    void HandleBounds()
    {
        Vector3 min = areaCenter - areaSize / 2;
        Vector3 max = areaCenter + areaSize / 2;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        pos.z = Mathf.Clamp(pos.z, min.z, max.z);

        transform.position = pos;
    }

    // 👀 LOOK DIRECTION
    void LookAtDirection(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    // ⏱️ TIMER
    void ResetTimer(float min, float max)
    {
        stateTimer = Random.Range(min, max);
    }

    // 🟨 DEBUG AREA (en editor)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
    
}
