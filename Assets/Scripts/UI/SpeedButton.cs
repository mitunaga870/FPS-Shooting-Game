using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InvasionPhase;
using Enums;
using Coffee.UIEffects;

public class SpeedButton : MonoBehaviour
{
    [SerializeField]
    private InvasionController invasionController;
    bool flag;

    public Component UIEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (invasionController.GameState == GameState.FastPlaying && flag == false) {
            UIEffect.GetComponent<UIEffect>().enabled = true;
        }

        if (invasionController.GameState != GameState.FastPlaying && flag == false) {
            UIEffect.GetComponent<UIEffect>().enabled = false;
        }
    }
}