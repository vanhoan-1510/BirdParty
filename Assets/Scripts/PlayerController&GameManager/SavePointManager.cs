using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class SavePointManager : MonoBehaviourPun
{
    private int requiredPlayersToActivate; // The number of players required to trigger the checkpoint and save
    private int playersTriggered = 0; // The number of players who triggered the checkpoint

    //public Transform savePoint;
    private Transform currentCheckpoint;
    public GameObject nextSavePoint;

    private List<int> playersInRoom = new List<int>();

    private void Start()
    {
        // Determine the number of players required to trigger the checkpoint
        Debug.Log("So nguoi trong rum: " + requiredPlayersToActivate);

        // Set the initial position as the save point for the player
        //savePoint.position = transform.position;
    }
    private void Update()
    {
        requiredPlayersToActivate = PhotonNetwork.PlayerList.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the PhotonView ID of the player who triggered the checkpoint
            int playerID = other.GetComponent<PhotonView>().ViewID;
            if (!playersInRoom.Contains(playerID))
            {
                playersInRoom.Add(playerID);
                playersTriggered++;

                // If the number of players who triggered the checkpoint is equal to the required number,
                // save the checkpoint for all players and reset the count of triggered players to 0
                if (playersTriggered >= requiredPlayersToActivate)
                {
                    currentCheckpoint = transform;
                    SaveCheckpointForAllPlayers();
                    Debug.Log("Checkpoint activated and saved for all players!");
                    Destroy(gameObject);
                    playersTriggered = 0;
                    nextSavePoint.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the PhotonView ID of the player who exited the checkpoint trigger
            int playerID = other.GetComponent<PhotonView>().ViewID;
            if (playersInRoom.Contains(playerID))
            {
                playersInRoom.Remove(playerID);
                playersTriggered--;
            }
        }
    }

    private void SaveCheckpointForAllPlayers()
    {
        // Save the checkpoint for all players in the room
        foreach (int playerID in playersInRoom)
        {
            PhotonView playerView = PhotonView.Find(playerID);
            if (playerView != null && playerView.IsMine)
            {
                playerView.RPC("SaveCheckpoint", RpcTarget.All, currentCheckpoint.position);
            }
        }
    }
}
