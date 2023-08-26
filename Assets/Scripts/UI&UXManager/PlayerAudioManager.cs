using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerAudioManager : MonoBehaviourPunCallbacks
{
    public static PlayerAudioManager Instance;
    private AudioListener audioListener;
    private AudioListener localAudioListener;
    public AudioSource[] playerAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        audioListener = GetComponent<AudioListener>();
        playerAudioSource = GetComponents<AudioSource>();
        AudioManager.Instance.playerAudioSources = playerAudioSource;
    }

    private void OnDestroy()
    {
        if (localAudioListener != null)
        {
            Destroy(localAudioListener);
        }
    }

    public void EndingFlySound()
    {
        StartCoroutine(EndFlyingSound());
    }

    public IEnumerator EndFlyingSound()
    {
        AudioManager.Instance.playerAudioSources[4].enabled = true;
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.playerAudioSources[4].enabled = false;
    }
}
