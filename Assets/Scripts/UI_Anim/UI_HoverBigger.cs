using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

//https://note.com/xxxx_kt_xxxx/n/n406b6ad20e5c
//https://qiita.com/8March/items/7e1cbc6cb0770f3cc492

public class UI_HoverBigger : MonoBehaviour
{
    [SerializeField] TMP_Text testText;
    [SerializeField] float up = 0.6f;
    [SerializeField] float randUP =0.1f;
    [SerializeField] float randDwn = 0.7f;
    //[SerializeField] float BiggerU = 80f;
    [SerializeField] float BiggerD = 80f;
    [SerializeField] float BiggerAU = 70f;
    //[SerializeField] float BiggerAD = 70f;
    [SerializeField] GameObject tmpInfo;
    [SerializeField] private int _seed;

    public void Start(){
        UnityEngine.Random.InitState(_seed);
        float randValue = UnityEngine.Random.Range(randUP, randDwn);
        Transform obj = this.GetComponent<Transform>();
        //obj.DOPunchPosition(new Vector3(0, 0.5f, 0), 2f, 5, 1f).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
        obj.DOMove(new Vector3( 0f, up + randValue, 0.0f) , 2.0f ).SetLoops(-1, LoopType.Yoyo).SetRelative(true);

    }

    public void OnMouseEnter()
    {
        Debug.Log("ON");
        this.transform.localScale = new Vector3(BiggerD, BiggerD, BiggerD);
        tmpInfo.SetActive(true);
        
    }
    public void OnMouseExit()
    {
        Debug.Log("Out");
        this.transform.localScale = new Vector3(BiggerAU,BiggerAU, BiggerAU);
        tmpInfo.SetActive(false);
    }
    
    public void OnClick()
    {
        Debug.Log("Clicked!");
        //クリックされたときのシーン遷移に使用している
    }
}
