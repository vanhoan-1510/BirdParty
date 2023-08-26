using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomNameInput;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text roomName;
    public GameObject listRoomPanel;
    public GameObject createRoomPanel;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdate = 1f;
    float nextUpdateTime;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject startGameButton;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        roomNameInput.characterLimit = 15;
    }

    public void OnClickCreateRoom()
    {
        if(roomNameInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true});
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList(); 
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time > nextUpdateTime)
        {
            nextUpdateTime = Time.time + timeBetweenUpdate;
            UpdateRoomList(roomList);
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach (RoomInfo info in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(info.Name);
            roomItemList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
            return;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            startGameButton.SetActive(true);
            //hostRoomItem.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
            //hostRoomItem.SetActive(false);
        }
    }

    public void OnClickShowRoomList()
    {
        listRoomPanel.SetActive(true);
        createRoomPanel.SetActive(false);
    }

    public void OnClickBack()
    {
        listRoomPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    public void OnClickbackToMenu()
    {
        SceneManager.LoadScene("MainLobby");
    }

    public void OnClickStartGame()
    {
        PhotonNetwork.LoadLevel("Coop");
    }
}
