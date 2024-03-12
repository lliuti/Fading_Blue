using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToStartingMenu());
    }

    void OnPause()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator GoToStartingMenu()
    {
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene(0);
    }
}
