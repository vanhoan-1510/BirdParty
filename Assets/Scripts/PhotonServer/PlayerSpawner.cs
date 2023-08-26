using UnityEngine;
using Photon.Pun;
using Cinemachine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static PlayerSpawner Instance;
    public GameObject[] playerPrefab;

    public Transform[] spawnPoints;
    public GameObject newPlayer;
    public Image[] playerAvatar;

    public Vector3 lastCheckpointPosition;

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

        //show player avatar
        playerAvatar[playerAvatarIndex].gameObject.SetActive(true);

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