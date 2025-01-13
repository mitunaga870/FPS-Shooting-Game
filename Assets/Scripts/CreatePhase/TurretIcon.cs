using System;
using AClass;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CreatePhase
{
    public class TurretIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private ATurret turretPrefab;
        
        [SerializeField]
        private TextMeshProUGUI remainingCountText;
        
        private MazeCreationController _mazeCreationController;
        
        private DetailViewerController _detailViewerController;
        
        private int _remainingCount = 1;

        /** 生成して移動中のたれっと) */
        private ATurret _turret;

        private void Update()
        {
            // 残りの数を表示
            remainingCountText.text = _remainingCount.ToString();
            
            // 全部設置したら非表示
            if (_remainingCount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        // ReSharper disable once ParameterHidesMember
        public void Init(MazeCreationController mazeCreationController, DetailViewerController detailViewerController)
        {
            _mazeCreationController = mazeCreationController;
            _detailViewerController = detailViewerController;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _mazeCreationController.StartSettingTurret(turretPrefab);
        }

        public void OnDrag(PointerEventData eventData) {}

        public void OnEndDrag(PointerEventData eventData)
        {
            _mazeCreationController.EndSettingTurret();
            _remainingCount--;
        }

        public void IncreaseCount()
        {
            _remainingCount++;
            
            // 表示を戻す
            if (_remainingCount > 0)
                gameObject.SetActive(true);
        }

        public string GetTurretName()
        {
            return turretPrefab.GetTurretName();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_detailViewerController == null) return;
            
            _detailViewerController.ShowTurretDetail(turretPrefab);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_detailViewerController == null) return;
            
            _detailViewerController.CloseDetail();
        }
    }
}