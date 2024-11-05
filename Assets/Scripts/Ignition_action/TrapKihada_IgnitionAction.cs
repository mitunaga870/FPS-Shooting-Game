using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class TrapKihada_IgnitionAction : MonoBehaviour
{
    public void IgnitionAction()
    {
        var cuurentPos = transform.position;
        //1秒間に360度回転させる
        //https://kingmo.jp/kumonos/dotween-rotate360loop/
        transform
            .DOLocalRotate(new Vector3(0, 720, 0),0.5f,RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
}
