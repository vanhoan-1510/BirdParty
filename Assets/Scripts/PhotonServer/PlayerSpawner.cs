using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;
    public GameObject[] playerPrefab;
    public Transform[] spawnPoints;
    public GameObject newPlayer;

    private void Start()
    {
        PlayerSpawn();
    }

    void PlayerSpawn()
    {
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
        else
        {
            Debug.LogError("Cinemachine freelook camera not found");
        }
    }
}
