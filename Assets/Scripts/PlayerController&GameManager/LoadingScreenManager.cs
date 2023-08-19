using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoadingScreenManager : MonoBehaviourPunCallbacks
{
    public static LoadingScreenManager Instance;
    public GameObject loadingScreen;
    private string targetSceneName;
    public float loadingTime;

    public GameObject loadingWheel;
    public float wheelSpeed;
    private bool isLoading;


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
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        //foreach (int playerID in playersInRoom)
        //{
        //    PhotonView playerView = PhotonView.Find(playerID);
        //}
        isLoading = true;

        //photonView.RPC("ShowLoadingScreen", RpcTarget.All);
        ShowLoadingScreen();
        Debug.Log("OK k sao");
        yield return new WaitForSeconds(3f);
        StartCoroutine(WheelSpinRoutine());

        PhotonNetwork.LoadLevel(targetSceneName);
        yield return new WaitForSeconds(2f);
        //photonView.RPC("HideLoadingScreen", RpcTarget.All);
        HideLoadingScreen();

        isLoading = false;
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

    private IEnumerator WheelSpinRoutine()
    {
        while(isLoading)
        {
            loadingWheel.transform.Rotate(0, 0, wheelSpeed);
            yield return null;
        }
    }
}
