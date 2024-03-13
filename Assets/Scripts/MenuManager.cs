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
    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider musicSlider;

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
        menuCanvas = transform.Find("MenuCanvas").gameObject;
        optionsMenuCanvas = transform.Find("OptionsMenuCanvas").gameObject;

        masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        SFXSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();

        menuCanvas.SetActive(false);
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
        CloseAllMenus();
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
        CloseAllMenus();
        SaveManager.instance.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void CloseAllMenus()
    {
        isPaused = false;
        menuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(false);
    }

}
