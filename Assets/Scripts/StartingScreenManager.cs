using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingScreenManager : MonoBehaviour
{
    public static StartingScreenManager instance;

    [SerializeField] Button continueButton;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        continueButton.interactable = SaveManager.instance.PreviousGame();
    }

    public void Continue()
    {
        SaveManager.instance.Load();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Scene1");
    } 

    public void Options()
    {
        
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
