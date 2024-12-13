using System;
using System.Collections.Generic;
using Chat;
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
    [DefaultExecutionOrder(100)]
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
        
        [SerializeField]
        private GeneralS2SData generalS2SData;
        
        
        // かくしゅUI 
        [SerializeField]
        private GameObject turretUI;
        
        // 仮のメッセージボックス
        [SerializeField]
        private MessageBoxController messageBox;

        private void Start()
        {
            // ステージデータを取得
            var stageData = mazeCreationController.StageData;

            // カスタムデータがないなら何もしない
            if (stageData.StageCustomData == null) return;
                
            // スキルとタレットが禁止されている場合はUIを非表示にする
            if (!stageData.StageCustomData.IsAllowedToSetTurret)
            {
                turretUI.SetActive(false);
            }
            
            // スキップの場合はもうマップを開く
            if (stageData.StageCustomData.IsSkip)
            {
                mapController.ShowMap( false, true);
            }
            
            // メッセージを表示
            var messages = new System.Collections.Generic.List<string>();
            messages.Add(generalS2SData.MapNumber + " - (" + generalS2SData.CurrentMapColumn+", "+generalS2SData.CurrentMapRow + ")");
            messages.Add("ステージ"+ stageData.stageName+"を開始します。\n 通路を作成してください");
            
            messageBox.SetMessages(messages.ToArray());
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
                messageBox.SetMessage("通路がつながっていません");
                
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