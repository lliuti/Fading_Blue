using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneController : MonoBehaviour
{
    [SerializeField] private bool isNextScene; 
    private bool onTriggerRange = false;
    private bool canInteract = false;

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
        if (onTriggerRange && canInteract) {
            TriggerScene(isNextScene);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<PlayerController>().collectedFirstCrystal) canInteract = true;
        onTriggerRange = true;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        onTriggerRange = false;
    }

}
