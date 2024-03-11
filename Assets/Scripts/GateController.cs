using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public static GateController instance;
    [SerializeField] private AudioClip gateOpeningClip;
    private Animator animator;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenGate()
    {
        animator.SetTrigger("Open");
        SoundFXManager.instance.PlaySoundFXClip(gateOpeningClip, transform, 1f);
    }

}
