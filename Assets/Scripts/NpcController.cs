using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private string[] dialogue;
    [SerializeField] private float typeSpeed;
    private GameObject dialogueCanvas;
    private TextMeshProUGUI dialogueText;
    private int dialogueIndex;
    private bool playerOnRange = false;
    private bool inDialogue = false;
    private SpriteRenderer spriteRenderer;
    private bool hasNewDialogue = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        dialogueCanvas = transform.parent.transform.Find("DialogueCanvas").gameObject;
        dialogueCanvas.SetActive(false);

        dialogueText = dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "";

        hasNewDialogue = dialogue.Length > 0;
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
            hasNewDialogue = false;
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

    void LookToPlayer(float playerX)
    {
        float distanceToPlayer = transform.position.x - playerX;
        if (distanceToPlayer < 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }

    void ChecksIfCollectedCrystal(bool collectedFirstCrystal)
    {
        if (gameObject.CompareTag("GateKeeper")) {
            if (collectedFirstCrystal) {
                dialogue = new string[] {
                    "What is... that?", 
                    "How did... you change your... light?", 
                    "Well. Does not matter!", 
                    "At least now I'm allowed to let the last of your kind leave.", 
                    "I don't know what you have done but you still are blue.", 
                    "And will never be one of us."
                };
                hasNewDialogue = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        ChecksIfCollectedCrystal(other.GetComponent<PlayerController>().collectedCrystal);
        
        if (!other.CompareTag("Player")) return;
        if (hasNewDialogue) interactionIndicator.SetActive(true);
        playerOnRange = true;

        LookToPlayer(other.transform.position.x);
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;
        playerOnRange = false;

        interactionIndicator.SetActive(false);
        LookToPlayer(other.transform.position.x);
        StopAllCoroutines();
        ClearDialogue();
    }

}
