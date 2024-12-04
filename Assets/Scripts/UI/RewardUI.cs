using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InvasionPhase;
using Enums;

public class RewardUI : MonoBehaviour
{
    [SerializeField]
    private InvasionController invasionController;
    bool flag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (invasionController.GameState == GameState.Clear && flag ==false) {
            gameObject.SetActive(true);
        }
    }
}
