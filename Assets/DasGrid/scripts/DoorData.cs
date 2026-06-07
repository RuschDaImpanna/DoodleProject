using UnityEngine;

public class DoorData : MonoBehaviour
{
    public string doorID;
    public string destinationRoom;
    public string destinationDoor;

    public void Initialize(string id, string destRoom, string destDoor)
    {
        doorID = id;
        destinationRoom = destRoom;
        destinationDoor = destDoor;
    }
}