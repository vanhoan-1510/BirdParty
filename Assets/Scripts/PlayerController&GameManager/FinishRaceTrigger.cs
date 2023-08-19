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
    public Text timeScore;

    public Timer timer;
    public string leaderboardKey = "birdpartygame_leaderboard";

    private string roomMembersNames = "";
    int maxScore = 5;
    public Text[] PlayerNameList;
    public Text[] PlayerScoreList;
 
    public Text playerTriggeredText;


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
                //Time.timeScale = 0f;
                SubmitScore(currentTime);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
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
                    PlayerNameList[i].text = score[i].member_id;
                    if (score[i].score == 0)
                    {
                        PlayerScoreList[i].text = "0";
                    }
                    else if (score[i].score / 100  < 10)
                    {
                        PlayerScoreList[i].text = "0" + score[i].score / 100 + " : " + score[i].score % 100;
                    }
                    else if (score[i].score / 100 == 0)
                    {
                        PlayerScoreList[i].text = "00" + score[i].score / 100 + " : " + score[i].score % 100;
                    }
                    else if (score[i].score % 100  == 0)
                    {
                        PlayerScoreList[i].text = score[i].score / 100 + " : 00";
                    }
                    else
                    {
                        PlayerScoreList[i].text = score[i].score / 100 + " : " + score[i].score % 100;
                    }
                }

                if(score.Length < maxScore)
                {
                    for (int i = score.Length; i < maxScore; i++)
                    {
                        PlayerNameList[i].text =  "None";
                        PlayerScoreList[i].text = "00 : 00";
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
