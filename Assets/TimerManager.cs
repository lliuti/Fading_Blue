using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    public bool increaseTimer = false;
    public float timer = 0f;
    public string parsedTime;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {   
        increaseTimer = true;        
    }

    void Update()
    {
        if (increaseTimer) timer += 1 * Time.deltaTime;
    }
    public void StopTimer()
    {
        increaseTimer = false;
        TimeSpan time = TimeSpan.FromSeconds(timer);
        string str = time.ToString(@"mm\:ss\:fff");
        parsedTime = str;
    }   
}
