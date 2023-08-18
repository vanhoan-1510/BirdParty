using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainLobbyUI : MonoBehaviourPunCallbacks
{
    public GameObject mainLobbyUI;
    public GameObject leaderBoard;
    public GameObject settings;
    public GameObject onButtonMusic;
    public GameObject offButtonMusic;

    public GameObject onButtonSound;
    public GameObject offButtonSound;

    public void OnClickLeaderBoard()
    {
        mainLobbyUI.SetActive(false);
        leaderBoard.SetActive(true);
    }

    public void OnClickSettings()
    {
        mainLobbyUI.SetActive(false);
        settings.SetActive(true);
    }

    public void OnClickBack()
    {
        mainLobbyUI.SetActive(true);
        leaderBoard.SetActive(false);
        settings.SetActive(false);
    }

    public void OnCLickChangeStateMusic()
    {
        if (onButtonMusic.activeSelf)
        {
            onButtonMusic.SetActive(false);
            offButtonMusic.SetActive(true);
        }
        else
        {
            onButtonMusic.SetActive(true);
            offButtonMusic.SetActive(false);
        }
    }

    public void OnCLickChangeStateSound()
    {
        if (onButtonSound.activeSelf)
        {
            onButtonSound.SetActive(false);
            offButtonSound.SetActive(true);
        }
        else
        {
            onButtonSound.SetActive(true);
            offButtonSound.SetActive(false);
        }
    }

    public void OnLickQuitGame()
    {
        Application.Quit();
    }

    public void OnGoToRoomLobby()
    {
        SceneManager.LoadScene("RoomLobby");
    }
}
