using AClass;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CreatePhase
{
    public class TurretIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private ATurret turretPrefab;

        [SerializeField]
        private MazeCreationController mazeCreationController;

        /** 生成して移動中のたれっと) */
        private ATurret _turret;

        public void OnBeginDrag(PointerEventData eventData)
        {
            mazeCreationController.StartSettingTurret(turretPrefab);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            mazeCreationController.EndSettingTurret();
        }
    }
}