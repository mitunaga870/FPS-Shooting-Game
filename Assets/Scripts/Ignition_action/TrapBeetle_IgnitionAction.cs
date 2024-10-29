using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class TrapBeetle_IgnitionAction : MonoBehaviour
{
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 0;
    [SerializeField] private float Z = 0;
    [SerializeField] private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.RightArrow)) {
            //現在の位置から（X,Y,Z）,time)だけ移動
           transform.DOMove(new Vector3(X,Y,Z), time).SetRelative(true).SetEase(Ease.InOutQuart);
        }
    }
}
