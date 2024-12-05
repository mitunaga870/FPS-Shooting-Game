using UnityEngine;
using UnityEngine.EventSystems; // EventSystemsを使用するために必要

public class RewardUIMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // マウスが要素上に入ったときに呼ばれるメソッド
    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    // マウスが要素から離れたときに呼ばれるメソッド
    public void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(0.96f, 0.96f, 0.96f);
    }
}