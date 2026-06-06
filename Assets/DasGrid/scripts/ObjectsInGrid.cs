using UnityEngine;

public class ObjectsInGrid
{
    public GameObject entity;
    public Vector3 rotation;

    public bool IsEmpty => entity == null;
    public ObjectsInGrid()
    {
        entity = null;
        rotation = Vector3.zero;
    }
    public override string ToString()
    {
        if (IsEmpty) return "empty";
        string name = entity != null ? entity.name : "?";
        return $"{name}\n{rotation.y:F0}°";
    }
}
