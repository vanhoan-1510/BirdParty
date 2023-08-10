using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;

public class FinishRaceTrigger : MonoBehaviourPun
{
    private int requiredPlayersToActivate; // The number of players required to trigger the checkpoint and save
    private int playersTriggered = 0; // The number of players who triggered the checkpoint

    private List<int> playersInRoom = new List<int>();

    public GameObject winningGame;

    public LeaderboardController leaderboardController;
    public Timer timer;

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
        //Debug.Log("thoi gian la: " + timer.timeToPost);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            int playerID = other.GetComponent<PhotonView>().ViewID;
            if (!playersInRoom.Contains(playerID))
            {
                playersInRoom.Add(playerID);
                playersTriggered++;
                if (playersTriggered >= requiredPlayersToActivate)
                {
                    FinishRaceForAllPlayer();
                    Debug.Log("Finish race for all players!");
                    Destroy(gameObject);
                    playersTriggered = 0;
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

    private void FinishRaceForAllPlayer()
    {
        // Save the checkpoint for all players in the room
        foreach (int playerID in playersInRoom)
        {
            PhotonView playerView = PhotonView.Find(playerID);
            if (playerView != null && playerView.IsMine)
            {
                int currentTime = PlayerPrefs.GetInt("timeToPost");
                winningGame.SetActive(true);
                //Time.timeScale = 0f;
                leaderboardController.SubmitScoreRoutine(currentTime);
                Debug.Log(currentTime);

            }
        }
    }
}
