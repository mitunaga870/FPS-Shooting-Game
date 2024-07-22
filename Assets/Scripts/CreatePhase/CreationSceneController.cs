using System;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CreatePhase
{
    /**
     * 制作フェーズでの全体動作を管理するクラス
     */
    public class CreationSceneController : MonoBehaviour
    {
        /** シーン間のデータ共有オブジェクト */
        [SerializeField] private CreateToInvasionData createToInvasionData;

        /** 迷路作成コントローラ */
        [SerializeField] private MazeCreationController mazeCreationController;

        /** ステージ情報 */
        [SerializeField] private StageData stageData;

        /**
         * やめるときの保存処理
         */
        private void OnApplicationQuit()
        {
            SaveController.SaveTileData(mazeCreationController.GetTileData());
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
                Debug.Log("迷路がつながってないよ");
                return;
            }

            // ========= シーン間のデータ共有オブジェクト関連 =========
            // 迷路情報を取得
            mazeCreationController.SetS2SData();

            // 迷路情報が正しいか確認
            if (!(
                    stageData.OneOneMazeColumns == createToInvasionData.GetMazeColumn() &&
                    stageData.OneOneMazeRows == createToInvasionData.GetMazeRow() &&
                    stageData.OneOneTrapCount == createToInvasionData.GetTrapCount()
                ))
            {
                return;
            }

            // ========= 侵攻フェーズに移動 =========
            SceneManager.LoadScene("InvasionPhase");
        }
    }
}