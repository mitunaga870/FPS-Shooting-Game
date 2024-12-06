using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InvasionPhase;
using Enums;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField]
    private InvasionController invasionController;
    bool flag;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;


    // Update is called once per frame
    void Update()
    {
        if (invasionController.GameState == GameState.Clear && flag ==false) {
            gameObject.SetActive(true);
        }
    }


    public void OnItem1Button() {
        //一番左のトラップが押されたら
        item1.GetComponent<Image>().enabled = false;
        item2.GetComponent<Image>().enabled = true;
        item3.GetComponent<Image>().enabled = true;
    }

    public void OnItem2Button() {
        //真ん中のトラップが押されたら
        item1.GetComponent<Image>().enabled = true;
        item2.GetComponent<Image>().enabled = false;
        item3.GetComponent<Image>().enabled = true;
    }

    public void OnItem3Button() {
        //一番右のトラップが押されたら
        item1.GetComponent<Image>().enabled = true;
        item2.GetComponent<Image>().enabled = true;
        item3.GetComponent<Image>().enabled = false;
    }
}
