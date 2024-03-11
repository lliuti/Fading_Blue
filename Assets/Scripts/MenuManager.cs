using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("UI")]
    public bool isPaused = false;
    private GameObject menuCanvas;
    private GameObject optionsMenuCanvas;
    private Slider masterSlider;
    private Slider SFXSlider;
    private Slider musicSlider;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        SFXSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        
        masterSlider.value = MixerManager.instance.masterLevel;
        SFXSlider.value = MixerManager.instance.SFXLevel;
        musicSlider.value = MixerManager.instance.musicLevel;

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
        SaveManager.instance.Save();
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
    
    public void GoToStartingScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void CloseAllMenus()
    {
        menuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
    }

}
