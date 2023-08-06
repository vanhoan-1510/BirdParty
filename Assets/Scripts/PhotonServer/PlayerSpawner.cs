using UnityEngine;
using Photon.Pun;
using Cinemachine;
using System.Collections;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefab;
    public Transform[] spawnPoints;
    public GameObject newPlayer;
    public Vector3 lastCheckpointPosition;

    private void Start()
    {
        PlayerSpawn();
    }

    private void Update()
    {
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
    }

    //[PunRPC]
    //public void SaveCheckpoint(Vector3 checkpointPosition)
    //{
    //    // Save the checkpoint position for the player
    //    lastCheckpointPosition = checkpointPosition;
    //    Debug.Log("Checkpoint saved for player: " + photonView.Owner.NickName);
    //}

    //public void RespawnAtLastCheckpoint()
    //{
    //    if (transform.position.y < -20f)
    //    {
    //        Destroy(player);
    //        StartCoroutine(RespawnPlayerAfterTime(1f));
    //        Debug.Log("Player respawned at last checkpoint: " + photonView.Owner.NickName);
    //    }
    //}

    //private IEnumerator RespawnPlayerAfterTime(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    PhotonNetwork.Instantiate(playerToSpawn.name, lastCheckpointPosition, Quaternion.identity);
    //}
}