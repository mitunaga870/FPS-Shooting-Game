using System;
using Enums;
using Map;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CreatePhase
{
    /**
     * 制作フェーズでの全体動作を管理するクラス
     */
    public class CreationSceneController : MonoBehaviour
    {
        /** シーン間のデータ共有オブジェクト */
        [SerializeField]
        private CreateToInvasionData createToInvasionData;

        /** 迷路作成コントローラ */
        [SerializeField]
        private MazeCreationController mazeCreationController;
        
        /** マップコントローラ */
        [SerializeField]
        private MapController mapController;

        /** ステージ情報 */
        [FormerlySerializedAs("stageData")]
        [SerializeField]
        private StageObject stageObject;
        
        // かくしゅUI 
        [SerializeField]
        private GameObject skillUI;
        [SerializeField]
        private GameObject turretUI;

        private void Start()
        {
            // ステージデータを取得
            var stageData = mazeCreationController.StageData;

            // カスタムデータがないなら何もしない
            if (stageData.StageCustomData == null) return;
                
            // スキルとタレットが禁止されている場合はUIを非表示にする
            if (!stageData.StageCustomData.IsAllowedToUseSkill)
            {
                skillUI.SetActive(false);
            }
            if (!stageData.StageCustomData.IsAllowedToSetTurret)
            {
                turretUI.SetActive(false);
            }
            
            // スキップの場合はもうマップを開く
            if (stageData.StageCustomData.IsSkip)
            {
                mapController.ShowMap( false, true);
            }
        }

        /**
         * やめるときの保存処理
         */
        private void OnApplicationQuit()
        {
            // セーブデータを保存
            SaveController.SavePhase(Phase.Create);
            SaveController.SaveStageData(mazeCreationController.StageData);
            SaveController.SaveTileData(mazeCreationController.GetTileData());
            SaveController.SaveTrapData(mazeCreationController.TrapData);
            SaveController.SaveTurretData(mazeCreationController.TurretData);
        }

        /**
         * 侵攻フェーズに移動する
         */
        public void GoToInvasionPhase()
        {
            // 迷路がつながってるか確認
            var shortestPath = mazeCreationController.GetShortestS2GPath();
            if (shortestPath == null)
            {
                // TODO: 迷路が不通のダイアログを出す。
                Debug.Log("迷路がつながってないよ");
                return;
            }

            // ========= シーン間のデータ共有オブジェクト関連 =========
            mazeCreationController.SetS2SData();
            createToInvasionData.IsInvasion = true;
            createToInvasionData.StageData = mazeCreationController.StageData;

            // ========= 侵攻フェーズに移動 =========
            SceneManager.LoadScene("InvasionPhase");
        }
    }
}