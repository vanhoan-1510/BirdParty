using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;
using LootLocker.Requests;
using Photon.Realtime;

public class FinishRaceTrigger : MonoBehaviourPun
{
    private int requiredPlayersToActivate; // The number of players required to trigger the checkpoint and save
    private int playersTriggered = 0; // The number of players who triggered the checkpoint

    private List<int> playersInRoom = new List<int>();

    public GameObject winningGame;

    public Timer timer;
    public string leaderboardKey = "birdpartygame_leaderboard";

    private string roomMembersNames = "";
    int maxScore = 10;
    public Text[] Entries;

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
        //ShowScore();
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
                SubmitScore(currentTime);
                Debug.Log("thoi gian: " +currentTime);

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

    public void ShowScore()
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, maxScore, (response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] score = response.items;

                for (int i = 0; i < score.Length; i++)
                {
                    Entries[i].text = score[i].rank + ". " +score[i].member_id + " " +  score[i].score;
                }

                if(score.Length < maxScore)
                {
                    for (int i = score.Length; i < maxScore; i++)
                    {
                        Entries[i].text = (i + 1).ToString() + ".   None";
                    }
                }
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
