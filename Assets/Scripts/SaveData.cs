using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int sceneIndex;

    public SaveData()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
