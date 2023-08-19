using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;

public class SavePointManager : MonoBehaviourPun
{
    private int requiredPlayersToActivate; // The number of players required to trigger the checkpoint and save
    private int playersTriggered = 0; // The number of players who triggered the checkpoint

    //public Transform savePoint;
    private Transform currentCheckpoint;
    public GameObject nextSavePoint;

    private List<int> playersInRoom = new List<int>();

    public Text playerTriggeredText;

    private void Update()
    {
        requiredPlayersToActivate = PhotonNetwork.PlayerList.Length;
        playerTriggeredText.text = playersTriggered + "/" + requiredPlayersToActivate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
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
