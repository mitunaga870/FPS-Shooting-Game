using System;
using AClass;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CreatePhase
{
    public class TurretIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private ATurret turretPrefab;
        
        [SerializeField]
        private TextMeshProUGUI _remainingCountText;
        
        private MazeCreationController mazeCreationController;
        
        private int _remainingCount = 1;

        /** 生成して移動中のたれっと) */
        private ATurret _turret;

        private void Update()
        {
            // 残りの数を表示
            _remainingCountText.text = _remainingCount.ToString();
            
            // 全部設置したら非表示
            if (_remainingCount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        // ReSharper disable once ParameterHidesMember
        public void Init(MazeCreationController mazeCreationController)
        {
            this.mazeCreationController = mazeCreationController;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mazeCreationController.StartSettingTurret(turretPrefab);
        }

        public void OnDrag(PointerEventData eventData) {}

        public void OnEndDrag(PointerEventData eventData)
        {
            mazeCreationController.EndSettingTurret();
            _remainingCount--;
        }

        public void IncreaseCount()
        {
            _remainingCount++;
        }

        public string GetTurretName()
        {
            return turretPrefab.GetTurretName();
        }
    }
}