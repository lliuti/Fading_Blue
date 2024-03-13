using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    
    private string path;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        };
        
        path = Application.persistentDataPath + "/acerola_0.ls";
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0) Save();
    }

    public bool PreviousGame()
    {
        return File.Exists(path);
    }

    public void Save()
    {
        SaveData data = new SaveData(
            MenuManager.instance.masterSlider.value, 
            MenuManager.instance.SFXSlider.value, 
            MenuManager.instance.musicSlider.value
        );

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Load()
    {
        if (!PreviousGame()) return;

        BinaryFormatter binaryFormatter = new BinaryFormatter(); 
        FileStream stream = new FileStream(path, FileMode.Open);
        SaveData data = binaryFormatter.Deserialize(stream) as SaveData;

        if (data.sceneIndex != SceneManager.GetActiveScene().buildIndex) {
            SceneManager.LoadScene(data.sceneIndex, LoadSceneMode.Single);
        }

        stream.Close();
        
        MenuManager.instance.masterSlider.value = data.masterLevel;
        MenuManager.instance.SFXSlider.value = data.SFXLevel;
        MenuManager.instance.musicSlider.value = data.musicLevel;
        // yes i know i can do better but god knows 
        // how tired of this freaking sound options i am. 
        MixerManager.instance.SetMasterVolume(data.masterLevel);
        MixerManager.instance.SetSFXVolume(data.SFXLevel);
        MixerManager.instance.SetMusicVolume(data.musicLevel);
    }
}
