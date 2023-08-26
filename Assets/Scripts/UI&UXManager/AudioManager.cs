using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioListener audioListener;

    public static AudioManager Instance;

    public Sound[] musicSounds, fxSounds;

    public AudioSource musicSource, sfxSource;
    public AudioSource[] playerAudioSources;

    int musicState;
    int soundState;

    private void Awake()
    {
        audioListener = GetComponent<AudioListener>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("MainLobbyMusic");

        musicState = PlayerPrefs.GetInt("MusicState");
        soundState = PlayerPrefs.GetInt("SoundState");

        if (musicState == 0)
        {
            MuteMusic();
        }

        if (soundState == 0)
        {
            MuteSFX();
        }
        //StopMusic("PlayGameMusic");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        musicSource.clip = s.audioClip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(fxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            sfxSource.PlayOneShot(s.audioClip);
        }
    }

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void MuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        for (int i = 0; i < playerAudioSources.Length; i++)
        {
            playerAudioSources[i].mute = !playerAudioSources[i].mute;
        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        musicSource.clip = s.audioClip;
        musicSource.Stop();
    }
}
