using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CrystalController : MonoBehaviour
{

    [SerializeField] private AudioClip collectingClip;
    [SerializeField] private bool blueCrystal = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();

        Destroy(gameObject);

        if (SceneManager.GetActiveScene().name != "Scene1" && !blueCrystal) GateController.instance.OpenGate();

        CollectedCrystal(player);
    }

    void CollectedCrystal(PlayerController player)
    {
        player.collectedCrystal = true;
        player.crystalParticles.Play();
    
        SoundFXManager.instance.PlaySoundFXClip(collectingClip, transform, 1f);

        Light2D playerLight = player.transform.Find("Light 2D").GetComponent<Light2D>();
        if (blueCrystal) {
            playerLight.color = new Color(50f/255, 50f/255, 255f/255, 255f/255);
            playerLight.intensity = 2f;
            playerLight.pointLightOuterRadius = 5f;

            Light2D globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
            globalLight.intensity = 1;
            globalLight.color = new Color(255f/255, 255f/255, 255f/255, 255f/255);
        } else {
            playerLight.color = new Color(255f/255, 1f/255, 1f/255, 255f/255);
        }
    }
}
