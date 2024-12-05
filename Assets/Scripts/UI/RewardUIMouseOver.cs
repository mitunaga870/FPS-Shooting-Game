using UnityEngine;
using UnityEngine.EventSystems; // EventSystems���g�p���邽�߂ɕK�v

public class RewardUIMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // �}�E�X���v�f��ɓ������Ƃ��ɌĂ΂�郁�\�b�h
    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    // �}�E�X���v�f���痣�ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(0.96f, 0.96f, 0.96f);
    }
}