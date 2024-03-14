using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimerManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI runText;
    [SerializeField] TextMeshProUGUI runTextValue;

    void Start()
    {
        if (TimerManager.instance != null) {
            runTextValue.text = TimerManager.instance.parsedTime;
        } else {
            Destroy(gameObject);
        }
    }
}
