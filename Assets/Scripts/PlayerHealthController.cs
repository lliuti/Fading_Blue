using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip deathSFX;
    private bool isDying = false;
    private Vector2 respawnPos;

    void Start()
    {
        respawnPos = transform.position;
    }

    void Update()
    {
        HandleKillzone();
    }

    void HandleKillzone()
    {
        if (transform.position.y < -9f && !isDying) Die();
    }

    public void Die()
    {
        isDying = true;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        SoundFXManager.instance.PlaySoundFXClip(deathSFX, transform, 1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(deathSFX.length);
        Time.timeScale = 1f;
        transform.position = respawnPos;
        isDying = false;
    }

    public void UpdateRespawnPos(Vector2 newPos)
    {
        respawnPos = newPos;
    }
}
