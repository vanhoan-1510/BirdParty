using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainLobbyUI : MonoBehaviourPunCallbacks
{
    public GameObject mainLobbyUI;
    public GameObject leaderBoard;
    public GameObject settings;
    public GameObject quitGameNoti;
    public GameObject howToPlay;

    public GameObject onButtonMusic;
    public GameObject offButtonMusic;
    public GameObject onButtonSound;
    public GameObject offButtonSound;

    public int musicState;
    public int soundState;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitGameNoti.SetActive(true);
            mainLobbyUI.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            howToPlay.SetActive(false);
        }
    }
    public void OnClickLeaderBoard()
    {
        mainLobbyUI.SetActive(false);
        leaderBoard.SetActive(true);
    }

    public void OnClickSettings()
    {
        musicState = PlayerPrefs.GetInt("MusicState");
        soundState = PlayerPrefs.GetInt("SoundState");

        if(musicState == 1)
        {
            onButtonMusic.SetActive(true);
            offButtonMusic.SetActive(false);
        }
        else
        {
            onButtonMusic.SetActive(false);
            offButtonMusic.SetActive(true);

        }

        if(soundState == 1)
        {
            onButtonSound.SetActive(true);
            offButtonSound.SetActive(false);
        }
        else
        {
            onButtonSound.SetActive(false);
            offButtonSound.SetActive(true);
        }

        AudioManager.Instance.PlaySFX("ClickButton");
        mainLobbyUI.SetActive(false);
        settings.SetActive(true);
    }

    public void OnClickBack()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        mainLobbyUI.SetActive(true);
        leaderBoard.SetActive(false);
        settings.SetActive(false);
        quitGameNoti.SetActive(false);
    }

    public void OnClickHowToPlay()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        howToPlay.SetActive(true);
    }

    public void OnCLickChangeStateMusic()
    {
        if (onButtonMusic.activeSelf && musicState == 1)
        {
            onButtonMusic.SetActive(false);
            offButtonMusic.SetActive(true);
            musicState = 0;
            PlayerPrefs.SetInt("MusicState", musicState);
        }
        else
        {
            onButtonMusic.SetActive(true);
            offButtonMusic.SetActive(false);
            musicState = 1;
            PlayerPrefs.SetInt("MusicState", musicState);
        }
        AudioManager.Instance.MuteMusic();
    }

    public void OnCLickChangeStateSound()
    {
        if(onButtonSound.activeSelf && soundState == 1)
        {
            onButtonSound.SetActive(false);
            offButtonSound.SetActive(true);
            soundState = 0;
            PlayerPrefs.SetInt("SoundState", soundState);
        }
        else
        {
            onButtonSound.SetActive(true);
            offButtonSound.SetActive(false);
            soundState = 1;
            PlayerPrefs.SetInt("SoundState", soundState);
        }
        AudioManager.Instance.MuteSFX();
    }

    public void OnClickQuitGame()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        Application.Quit();
    }

    public void OnGoToRoomLobby()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        SceneManager.LoadScene("RoomLobby");
    }
}
