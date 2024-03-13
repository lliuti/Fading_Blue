using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int sceneIndex;
    public float masterLevel;
    public float SFXLevel;
    public float musicLevel;

    public SaveData(float master, float SFX, float music)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        masterLevel = master;
        SFXLevel = SFX;
        musicLevel = music;
    }
}
