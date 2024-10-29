using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class TrapKihada_IgnitionAction : MonoBehaviour
{
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 1800;
    [SerializeField] private float Z = 0;
    [SerializeField] private float time = 4;
    public enum SampleEnum
    {
        None,
        A,
        B,
        C,
        D,
    }
    [SerializeField] private SampleEnum sampleEnum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKey (KeyCode.A	)) {
            //1秒間に360度回転させる
            //https://kingmo.jp/kumonos/dotween-rotate360loop/
            transform.DOLocalRotate(new Vector3(X,Y,Z),time,RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        }
    }
}
