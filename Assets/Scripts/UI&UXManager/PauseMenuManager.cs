using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    bool isPaused = false;
    public GameObject leaveGameConfirm;

    public GameObject onButtonMusic;
    public GameObject offButtonMusic;

    public GameObject onButtonSound;
    public GameObject offButtonSound;

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
        AudioManager.Instance.PlaySFX("ClickButton");
        pauseMenu.SetActive(false);
        isPaused = false;
        leaveGameConfirm.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClickLeaveGame()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainLobby");
        AudioManager.Instance.audioListener.enabled = true;
        AudioManager.Instance.PlayMusic("MainLobbyMusic");
    }

    public void OnlickShowConfirm() 
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        leaveGameConfirm.SetActive(true);
    }

    public void OnlickHideConfirm()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        leaveGameConfirm.SetActive(false);
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
        AudioManager.Instance.MuteMusic();
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
        AudioManager.Instance.MuteSFX();
    }
}
