using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalController : MonoBehaviour
{

    [SerializeField] private AudioClip collectingClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerController>().collectedCrystal = true;

        SoundFXManager.instance.PlaySoundFXClip(collectingClip, transform, 0.4f);
        Destroy(gameObject);

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Scene1") GateController.instance.OpenGate();
    }
}
