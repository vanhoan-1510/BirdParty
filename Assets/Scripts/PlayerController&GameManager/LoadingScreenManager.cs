using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LoadingScreenManager : MonoBehaviourPunCallbacks
{
    public static LoadingScreenManager Instance;
    public GameObject loadingScreen;
    private string targetSceneName;
    public float loadingTime;

    public GameObject loadingWheel;
    public float wheelSpeed;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        loadingScreen.SetActive(false);
    }

    public void LoadScreen(string sceneName)
    {
        targetSceneName = sceneName;
        photonView.RPC("SyncMusic", RpcTarget.All);
        StartCoroutine(LoadSceneRoutine());

    }

    [PunRPC]
    public void SyncMusic()
    {
        AudioManager.Instance.StopMusic("MainLobbyMusic");
        AudioManager.Instance.PlayMusic("PlayGameMusic");
        //AudioManager.Instance.audioListener.enabled = false;
    }

    private IEnumerator LoadSceneRoutine()
    {
        photonView.RPC("ShowLoadingScreen", RpcTarget.All);
        //ShowLoadingScreen();
        Debug.Log("OK k sao");
        yield return new WaitForSeconds(3f);

        PhotonNetwork.LoadLevel(targetSceneName);
        yield return new WaitForSeconds(2f);
        photonView.RPC("HideLoadingScreen", RpcTarget.All);
        //HideLoadingScreen();
    }

    [PunRPC]
    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    [PunRPC]
    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}
