using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeSceneController : MonoBehaviour
{
    [SerializeField] private bool isNextScene; 
    [SerializeField] private bool isWalkable = true;
    [SerializeField] private bool needsCrystal = true;
    [SerializeField] private Animator animator;
    private GameObject interactionIndicator;
    private bool onTriggerRange = false;
    private bool canInteract = false;

    void Start()
    {
        if (!isWalkable) {
            interactionIndicator = transform.Find("InteractionIndicator").gameObject;
            interactionIndicator.SetActive(false);
        }
    }

    void TriggerScene(bool isNextScene) {
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;
        int nextScene = isNextScene ? currentSceneIndex + 1 : currentSceneIndex - 1;
        StartCoroutine(FadeOut(nextScene));
    }

    IEnumerator FadeOut(int scene)
    {
        animator.SetTrigger("Out");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
        if (scene == 10 && TimerManager.instance != null) TimerManager.instance.StopTimer();
    }
    
    void OnInteract()
    {
        if (onTriggerRange && canInteract) {
            TriggerScene(isNextScene);
        }
    }

    void ShowIndicator()
    {
        interactionIndicator.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<PlayerController>().collectedCrystal || !needsCrystal) canInteract = true;
        if (isWalkable && canInteract) TriggerScene(isNextScene);
        if (!isWalkable && canInteract) ShowIndicator();
        onTriggerRange = true;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        onTriggerRange = false;
    }

}
