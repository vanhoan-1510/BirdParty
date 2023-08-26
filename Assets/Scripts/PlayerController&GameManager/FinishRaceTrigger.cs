using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;
using LootLocker.Requests;
using Photon.Realtime;

public class FinishRaceTrigger : MonoBehaviourPun
{
    public static FinishRaceTrigger Instance;
    private int requiredPlayersToActivate; // The number of players required to trigger the checkpoint and save
    private int playersTriggered = 0; // The number of players who triggered the checkpoint

    private List<int> playersInRoom = new List<int>();
    private string roomMembersNames = "";

    private string leaderboardKey = "birdpartygame_leaderboard";
    public GameObject winningGame;
    public Text timeScore;

    public Timer timer;
 
    public Text playerTriggeredText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //check lootlocker status
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });

        if (PhotonNetwork.InRoom)
        {
            UpdateRoomMembersList();
        }
    }
    private void Update()
    {
        requiredPlayersToActivate = PhotonNetwork.PlayerList.Length;
        
        playerTriggeredText.text = playersTriggered + "/" + requiredPlayersToActivate;

        if(playersTriggered == requiredPlayersToActivate)
        {
            playerTriggeredText.text =  " 0/" + requiredPlayersToActivate;
        }
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
                timeScore.text = currentTime / 100 + " : " + currentTime % 100;
                winningGame.SetActive(true);
                AudioManager.Instance.PlaySFX("WinGame");
                //Time.timeScale = 0f;
                SubmitScore(currentTime);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void SubmitScore(int timeScore)
    {
        LootLockerSDKManager.SubmitScore(roomMembersNames, timeScore, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    public void UpdateRoomMembersList()
    {
        roomMembersNames = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            roomMembersNames += player.NickName + ", ";
        }

        // Remove the trailing comma and space
        if (!string.IsNullOrEmpty(roomMembersNames))
        {
            roomMembersNames = roomMembersNames.Substring(0, roomMembersNames.Length - 2);
        }

        Debug.Log("Room Members: " + roomMembersNames);
    }
}
