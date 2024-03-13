using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField] private GameObject interactionIndicator;
    private Animator animator;
    private bool destroyed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.GetComponent<PlayerController>().collectedCrystal) return;
        
        interactionIndicator.SetActive(true);

        if (destroyed) return;
        
        destroyed = true;
        animator.SetTrigger("Break");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (interactionIndicator) interactionIndicator.SetActive(false);
    }
}
