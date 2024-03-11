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
    public bool existingGame = false;
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
        Scene level = SceneManager.GetActiveScene();
        int index = level.buildIndex;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, index);
        stream.Close();
    }

    public void Load()
    {
        if (!PreviousGame()) return;

        BinaryFormatter binaryFormatter = new BinaryFormatter(); 
        FileStream stream = new FileStream(path, FileMode.Open);
        int loadedIndex = (int)binaryFormatter.Deserialize(stream);
        stream.Close();

        if (!(loadedIndex == SceneManager.GetActiveScene().buildIndex)) {
            SceneManager.LoadScene(loadedIndex, LoadSceneMode.Single);
        }
    }
}
