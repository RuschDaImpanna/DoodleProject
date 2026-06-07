using UnityEngine;
using System.IO;

public class RoomTransition : MonoBehaviour
{
    private DoorData doorData;
    private structuremap structureAssembler;
    private interactablemap interactableAssembler;
    private Transform player;
    private bool isTransitioning = false;

private void Start()
{
    doorData = GetComponent<DoorData>();
    structureAssembler = FindFirstObjectByType<structuremap>();
    interactableAssembler = FindFirstObjectByType<interactablemap>();
    player = GameObject.FindWithTag("Player").transform;
}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"doorData.doorID: '{doorData.doorID}' dest room: '{doorData.destinationRoom}' dest door: '{doorData.destinationDoor}'");
        if (isTransitioning) return;
        if (other.CompareTag("Player"))
        {
            isTransitioning = true;
            LoadRoom(doorData.destinationRoom, doorData.destinationDoor);
        }
    }

    private void LoadRoom(string roomName, string destinationDoorID)
    {
        // destroy all current objects
        structureAssembler.ClearRoom();
        interactableAssembler.ClearRoom();

        // reload with new room maps
        structureAssembler.LoadRoom(roomName);
        interactableAssembler.LoadRoom(roomName);

        // move player to destination door
        MovePlayerToDoor(destinationDoorID);
    }
    private void MovePlayerToDoor(string destinationDoorID)
{
    // find all door objects in the scene
    DoorData[] allDoors = FindObjectsByType<DoorData>(FindObjectsSortMode.None);
    foreach (DoorData door in allDoors)
    {
        if (door.doorID == destinationDoorID)
        {
            player.position = door.transform.position + door.transform.forward * -1.8f;
            return;
        }
    }
}
}
