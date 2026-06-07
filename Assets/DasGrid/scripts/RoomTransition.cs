using UnityEngine;
using System.IO;

public class RoomTransition : MonoBehaviour
{
    private DoorData doorData;
    private structuremap structureAssembler;
    private interactablemap interactableAssembler;
    private Transform player;

private void Start()
{
    doorData = GetComponent<DoorData>();
    structureAssembler = FindFirstObjectByType<structuremap>();
    interactableAssembler = FindFirstObjectByType<interactablemap>();
    player = GameObject.FindWithTag("Player").transform;
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            player.position = door.transform.position;
            return;
        }
    }
}
}
