using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerController>().collectedCrystal = true;
        Destroy(gameObject);

        Scene scene = SceneManager.GetActiveScene();

        if (scene.buildIndex == 0) {
            GameObject doorLock = GameObject.Find("Lock");
            Destroy(doorLock);
        } else {
            GameObject gate = GameObject.Find("Gate");
            gate.GetComponent<Animator>().SetTrigger("Open");
        }

    }
}
