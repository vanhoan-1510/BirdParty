using UnityEngine;
using Photon.Pun;

public class PlayerAudioManager : MonoBehaviourPunCallbacks
{
    private AudioListener audioListener;

    private void Awake()
    {
        audioListener = GetComponent<AudioListener>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            audioListener.enabled = true;
        }
    }
}
