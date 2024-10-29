using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる

public class CircuitBreaker_IgnitionAnimation : MonoBehaviour
{
    //===== 定義領域 =====
    private Animator anim;  //Animatorをanimという変数で定義する

    [SerializeField] Material materialA;//material 1
    [SerializeField] Material materialB;

    // Start is called before the first frame update
    void Start()
    {
        //変数animに、Animatorコンポーネントを設定する
        anim = gameObject.GetComponent<Animator>();


        materialA.color = new Color(0,0,0);
        materialB.color = new Color(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.S)) {
            //https://qiita.com/sanpeita/items/3864578e852801c15f80
            
            //Bool型のパラメーターであるblRotをTrueにする
            anim.SetBool("Ent", true);

            materialA.color = new Color(50,0,0);
            materialB.color = new Color(50,0,0);

            // 3秒後にアニメーションの再生スピードが０になる
            //https://qiita.com/thorikawa/items/924d38f7eea0f71c5856
            DOVirtual.DelayedCall(9, () => anim.SetFloat("MovingSpeed", 0.0f));
            //22秒後にマテリアルの色を0に
            DOVirtual.DelayedCall(22, () => materialA.color = new Color(0,0,0));
            //22秒後にマテリアルの色を0に
            DOVirtual.DelayedCall(22, () => materialB.color = new Color(0,0,0));
        }
        if (Input.GetKey (KeyCode.L)) {
                anim.SetFloat("MovingSpeed", 1.0f); // 再開
        }

    }
}
