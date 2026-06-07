using UnityEngine;

public class ObjectsInGrid
{
    public string entity;
    public Vector3 rotation;
    public bool isEnergized = false;

    public bool IsEmpty => string.IsNullOrEmpty(entity);

    public ObjectsInGrid()
    {
        entity = null;
        rotation = Vector3.zero;
        isEnergized = false;
    }

    public override string ToString()
    {
        if (IsEmpty) return "empty";
        string name = entity != null ? entity : "?";
        return $"{name}\n{rotation.y:F0}°";
    }
}