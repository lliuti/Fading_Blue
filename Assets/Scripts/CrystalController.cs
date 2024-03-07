using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField] private GameObject doorLock; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerController>().collectedFirstCrystal = true;
        Destroy(doorLock);
        Destroy(gameObject);
    }
}
