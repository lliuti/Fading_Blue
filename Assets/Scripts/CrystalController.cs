using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerController>().collectedFirstCrystal = true;
        Destroy(gameObject);
    }
}
