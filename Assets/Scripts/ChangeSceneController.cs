using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneController : MonoBehaviour
{
    [SerializeField] private bool isNextScene; 
    private bool onTriggerRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TriggerScene(bool isNextScene) {
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;
        int nextScene = isNextScene ? currentSceneIndex + 1 : currentSceneIndex - 1;
        SceneManager.LoadScene(nextScene);
    }
    
    void OnInteract()
    {
        if (onTriggerRange) {
            TriggerScene(isNextScene);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        onTriggerRange = true;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        onTriggerRange = false;
    }

}
