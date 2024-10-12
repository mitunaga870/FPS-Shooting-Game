using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class TrapCorn_ActionIgnition : MonoBehaviour
{
     [SerializeField] private float time = 4;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.C	)) {
           DOTween.Sequence().Append(DOVirtual.DelayedCall(time, () => this.gameObject.SetActive(false))); 
        }
    }
}
