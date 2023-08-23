using UnityEngine;
using Photon.Pun;
using Cinemachine;
using System.Collections;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static PlayerSpawner Instance;
    public GameObject[] playerPrefab;

    public Transform[] spawnPoints;
    public GameObject newPlayer;

    public Vector3 lastCheckpointPosition;

    public AudioSource playerAudioSource;

    private void Start()
    {
        StartCoroutine(PlayerSpawn());
    }

    IEnumerator PlayerSpawn()
    {
        yield return new WaitForSeconds(8f);

        int playerAvatarIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
        GameObject playerToSpawn = playerPrefab[playerAvatarIndex];

        //int spawnIndex = GetNextSpawnIndex();
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];
        //Transform spawnPoint = spawnPoints[spawnIndex];

        newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

        playerAudioSource = newPlayer.GetComponent<AudioSource>();
        AudioManager.Instance.playerAudioSource = playerAudioSource;

        CinemachineFreeLook cinemachineCamera = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineFreeLook>();
        if (newPlayer != null)
        {
            Debug.Log("Player spawned");
            cinemachineCamera.Follow = newPlayer.transform;
            cinemachineCamera.LookAt = newPlayer.transform;
        }
    }

    //private int nextSpawnIndex = 0;

    //private int GetNextSpawnIndex()
    //{
    //    int index = nextSpawnIndex;
    //    nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Length;
    //    return index;
    //}
}