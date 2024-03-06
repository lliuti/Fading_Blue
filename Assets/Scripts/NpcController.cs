using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] dialogue;
    [SerializeField] private float typeSpeed;
    private int dialogueIndex;
    private bool playerOnRange = false;
    private bool inDialogue = false;

    void Start()
    {
        dialogueText.text = "";
    }

    void Update()
    {
        if (inDialogue) interactionIndicator.SetActive(false);
    }

    void OnInteract()
    {
        if (!playerOnRange) return;

        if (inDialogue) {
            StopAllCoroutines();
            NextLine();
            return;
        }

        if (dialogueCanvas.activeInHierarchy) {
            ClearDialogue();
            return;
        } 

        inDialogue = true;
        dialogueCanvas.SetActive(true);
        StartCoroutine(Typing());
    }

    void NextLine()
    {
        if (dialogueIndex < dialogue.Length - 1) {
            dialogueIndex++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        } else {
            ClearDialogue();
        };

    }

    void ClearDialogue()
    {
        dialogueText.text = "";
        dialogueIndex = 0;
        dialogueCanvas.SetActive(false);
        inDialogue = false;
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[dialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        if (dialogue.Length > 0) interactionIndicator.SetActive(true);
        playerOnRange = true;
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        playerOnRange = false;
        interactionIndicator.SetActive(false);
        ClearDialogue();
    }

}
