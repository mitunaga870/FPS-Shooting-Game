using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;
using TMPro;


public class CreditCSV : MonoBehaviour
{
    public GameObject Card;
    public Transform parent;

    [SerializeField] private SheetData sheetData;
     void Start()
     {
         //Debug.Log(sheetData.sheetDataRecord[0].name);
        Instantiate(Card,new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity,parent);
        //GameObject tmProObj = Card.transform.Find("TOP").gameObject;//子のTMProオブジェクト取得
        //TextMeshProUGUI  tmProObj = GameObject.Find("TOP");//子のTMProオブジェクト取得
        //tmProObj.text = "AHO";//テキスト生成
        //GameObject parent2 = grandParent.transform.Find("GameObject").gameObject;
        //GameObject child1  = parent.transform.Find("TOP").gameObejct;
        //GameObject child2  = grandparent.transform.Find("TOP/Sub").gameObejct;
        //child1.text = "AHO";//テキスト生成
        //child2.text = "AHO";//テキスト生成

     }

}
