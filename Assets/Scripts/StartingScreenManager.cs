using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingScreenManager : MonoBehaviour
{
    [SerializeField] Button continueButton;

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
    
    public void Quit()
    {
        Application.Quit();
    }
}
