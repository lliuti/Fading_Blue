using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGroundController : MonoBehaviour
{
    [SerializeField] private GameObject crystal;
    private Animator animator;
    private bool hasEntered = false;

    void Start()
    {
        animator = GetComponent<Animator>();        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player") || hasEntered) return;
        animator.SetTrigger("Disappear");
        hasEntered = true;
        if (crystal != null) crystal.SetActive(true);
    }
}
