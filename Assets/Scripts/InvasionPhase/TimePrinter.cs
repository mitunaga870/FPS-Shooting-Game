using System.Collections;
using System.Collections.Generic;
using InvasionPhase;
using TMPro;
using UnityEngine;

public class TimePrinter : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;

    [SerializeField] InvasionController sceneController;

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Time: " + sceneController.GameTime;
    }
}