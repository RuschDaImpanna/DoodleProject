using UnityEngine;

public class ColliderDoor : MonoBehaviour, IElectronic
{
    public bool isEnergized = false;
    public bool IsEnergized => isEnergized;
    
    private Collider doorCollider;

    private void Start()
    {
        doorCollider = GetComponent<Collider>();
    }

    public void ReceiveEnergy()
    {
        isEnergized = true;
        doorCollider.enabled = false; // open the door
    }

    public void ResetEnergy()
    {
        isEnergized = false;
        doorCollider.enabled = true; // close the door
    }
}