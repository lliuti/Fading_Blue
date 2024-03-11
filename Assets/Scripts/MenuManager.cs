using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("UI")]
    private GameObject menuCanvas;
    private GameObject optionsMenuCanvas;
    public bool isPaused = false;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else { 
            Destroy(gameObject);
        };
    }

    void Start()
    {
        menuCanvas = GameObject.Find("MenuCanvas");
        menuCanvas.SetActive(false);

        optionsMenuCanvas = GameObject.Find("OptionsMenuCanvas");
        optionsMenuCanvas.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        OpenMainMenu();
    }
    
    public void Unpause()
    {
        Time.timeScale = 1f;
        isPaused = false;
        CloseAllMenus();
    }

    public void Save()
    {
        SaveManager.instance.Save();
        Unpause();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        menuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
    }

    public void OpenMainMenu()
    {
        optionsMenuCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
    

    private void CloseAllMenus()
    {
        menuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
    }

}
