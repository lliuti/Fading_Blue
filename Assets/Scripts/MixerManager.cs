using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerManager : MonoBehaviour
{
    public static MixerManager instance;

    [SerializeField] private AudioMixer audioMixer;
    public float masterLevel = 1f;
    public float SFXLevel = 1f;
    public float musicLevel = 1f;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        };
    }

    public void SetMasterVolume(float level)
    {
        masterLevel = level;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        SFXLevel = level;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        musicLevel = level;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
}
