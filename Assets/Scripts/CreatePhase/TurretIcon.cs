using AClass;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CreatePhase
{
    public class TurretIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ATurret turretPrefab;
        [SerializeField] private MazeCreationController mazeCreationController;
        
        /** 生成して移動中のたれっと) */
        private ATurret _turret;

        public void OnBeginDrag(PointerEventData eventData)
        {
            mazeCreationController.IsSettingTurret = true;
            // ドラッグ開始時にタレットを生成
            _turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // メインカメラを取得できない場合は処理を中断
            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            
            // ドラッグ中のマウス位置を取得
            var v2Position = eventData.position;
            // 座標返還
            var v3Position = new Vector3(v2Position.x, 10f, v2Position.y);
            // TODO: スクリーンの座標系からワールドの座標系に合わせる。だいぶ大変そうだからドラッグ中はアイコンで設置字にタイルでいいかも（これならonEnterでマウス下のタイル多分とれる）
            var worldPosition = mainCamera.ScreenToWorldPoint(v2Position);
            Debug.Log(worldPosition);
            // ドラッグ中はタレットを移動
            _turret.transform.position = worldPosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            mazeCreationController.IsSettingTurret = false;
        }
    }
}
