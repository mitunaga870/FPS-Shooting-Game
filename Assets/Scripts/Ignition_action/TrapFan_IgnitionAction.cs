using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる


public class TrapFan_IgnitionAction : MonoBehaviour
{   
    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 0;
    [SerializeField] private float Z = 1800;
    [SerializeField] private string ActionKey = "U";
    [SerializeField] private float time = 4;
    [SerializeField] GameObject obj;
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
        obj.SetActive(false);   // 無効にする
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.F)) {
            obj.SetActive(true);    // 有効にする
            //time秒間にX,Y,Z度回転させる
            //https://kingmo.jp/kumonos/dotween-rotate360loop/
            transform.DOLocalRotate(new Vector3(X,Y,Z),time,RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
            DOTween.Sequence().Append(DOVirtual.DelayedCall(time, () => obj.SetActive(false))); //time goni mukou
        }
    }
}
