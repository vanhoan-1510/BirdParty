using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    bool isPaused = false;
    public GameObject leaveGameConfirm;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClickLeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainLobby");
    }

    public void OnlickShowConfirm() 
    {         
        leaveGameConfirm.SetActive(true);
    }

    public void OnlickHideConfirm()
    {
        leaveGameConfirm.SetActive(false);
    }
}
