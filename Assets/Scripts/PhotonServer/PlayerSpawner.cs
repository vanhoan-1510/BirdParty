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

    private void Start()
    {
        StartCoroutine(PlayerSpawn());
    }

    private void Update()
    {
    }

    IEnumerator PlayerSpawn()
    {
        yield return new WaitForSeconds(5f);
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];
        GameObject playerToSpawn = playerPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];   
        newPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

        CinemachineFreeLook cinemachineCamera = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineFreeLook>();
        if (newPlayer != null)
        {
            Debug.Log("Player spawned");
            cinemachineCamera.Follow = newPlayer.transform;
            cinemachineCamera.LookAt = newPlayer.transform;
        }
    }
}