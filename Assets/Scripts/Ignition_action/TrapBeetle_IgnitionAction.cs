using DG.Tweening;
using UnityEngine;

//DOTweenを使うときはこのusingを入れる

namespace Ignition_action
{
    public class TrapBeetleIgnitionAction : MonoBehaviour
    {
        /**
     * トラップの起動アクション
     * @param destination 移動先の座標
     * @param time 移動にかかる時間(sec)
     */
        public void IgnitionAction(
            Vector3 destination,
            float time
        ) {
            var prevPos = transform.position;
            //現在の位置から（X,Y,Z）,time)だけ移動
            transform.DOMove(destination, time).SetRelative(false).SetEase(Ease.InOutQuart).onComplete = () =>
            {
                //移動完了時もとの位置に戻す
                transform.position = prevPos;
            };
        }
    }
}
