using InvasionPhase;
using lib;
using UnityEngine;

//DOTweenを使うときはこのusingを入れる


namespace Ignition_action
{
    // ReSharper disable once InconsistentNaming
    public class CrocTrap_ActionIgnition : MonoBehaviour
    {
        [SerializeField] private float time = 4;
        //===== 定義領域 =====
        private Animator anim;  //Animatorをanimという変数で定義する

        // Start is called before the first frame update
        void Start()
        {
            //変数animに、Animatorコンポーネントを設定する
            anim = gameObject.GetComponent<Animator>();   
        }

        public void IgnitionAction()
        {
            //アニメーションを再生する
            anim.SetBool("RollBool", true);
            // 少し待ってからアニメーションを止める
            var delay = General.DelayCoroutine(
                0.5f
                , () => { anim.SetBool("RollBool", false); }
            );
            StartCoroutine(delay);
        }
    }
}
