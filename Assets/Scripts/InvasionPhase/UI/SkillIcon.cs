using System;
using AClass;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InvasionPhase.UI
{
    public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ASkill _skill;

        [SerializeField]
        private TextMeshProUGUI _skillRemainingCountText;
        
        private DetailViewerController _detailViewerController;
        
        private InvasionController _sceneController;
        
        /** スキルの残り使用回数*/
        private int _remainingCount = 1;

        private void Update()
        {
            _skillRemainingCountText.text = _remainingCount.ToString();
            
            // スキルの残り使用回数が0になったら非表示
            if (_remainingCount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void Init(
            InvasionController sceneController,
            DetailViewerController detailViewerController
        ) {
            _detailViewerController = detailViewerController;
            _sceneController = sceneController;
        }

        public void SetSelectPositionMode()
        {
            if (_sceneController == null) return;
            
            _sceneController.SetSkillMode(_skill);
        }

        public void IncreaseCount()
        {
            _remainingCount++;
        }

        public void DecreaseCount()
        {
            _remainingCount--;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_detailViewerController == null) return;
            
            _detailViewerController.ShowSkillDetail(_skill);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_detailViewerController == null) return;
            
            _detailViewerController.CloseDetail();
        }
    }
}
