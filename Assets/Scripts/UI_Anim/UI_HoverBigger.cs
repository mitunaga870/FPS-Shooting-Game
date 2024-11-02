using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//https://note.com/xxxx_kt_xxxx/n/n406b6ad20e5c
//https://qiita.com/8March/items/7e1cbc6cb0770f3cc492

public class UI_HoverBigger : MonoBehaviour
{
    RectTransform RectTransform_get;

    void Start()
    {
        RectTransform_get = gameObject.GetComponent<RectTransform>();
    }

    public void OnMouseEnter()
    {
        Debug.Log("ON");
        this.transform.localScale = new Vector3(80f, 80f, 80f);

    }
    public void OnMouseExit()
    {
        Debug.Log("Out");
        this.transform.localScale = new Vector3(70f,70f, 70f);
    }
    
    public void OnClick()
    {
        Debug.Log("Clicked!");
    }
}
