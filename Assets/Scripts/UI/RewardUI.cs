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

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

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

    public void OnItem1Button() {
        Debug.Log("�A�C�e��1�̃{�^���������ꂽ");
    }

    public void OnItem2Button() {
        Debug.Log("�A�C�e��2�̃{�^���������ꂽ");
    }

    public void OnItem3Button() {
        Debug.Log("�A�C�e��3�̃{�^���������ꂽ");
    }
}
