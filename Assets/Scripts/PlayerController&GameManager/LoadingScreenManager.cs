using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class LoadingScreenManager : MonoBehaviour
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
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        isLoading = true;

        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(WheelSpinRoutine());

        PhotonNetwork.LoadLevel(targetSceneName);
        yield return new WaitForSeconds(2f);
        loadingScreen.SetActive(false);

        isLoading = false;
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
