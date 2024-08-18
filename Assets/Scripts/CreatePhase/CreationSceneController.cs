using Enums;
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
        [SerializeField] private CreateToInvasionData createToInvasionData;

        /** 迷路作成コントローラ */
        [SerializeField] private MazeCreationController mazeCreationController;

        /** ステージ情報 */
        [FormerlySerializedAs("stageData")] [SerializeField]
        private StageObject stageObject;

        /**
         * やめるときの保存処理
         */
        private void OnApplicationQuit()
        {
            // セーブデータを保存
            SaveController.SavePhase(Phase.Create);
            SaveController.SaveTileData(mazeCreationController.GetTileData());
            SaveController.SaveTrapData(mazeCreationController.TrapData);
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
            mazeCreationController.SetS2SData();
            createToInvasionData.IsInvasion = true;

            // ========= 侵攻フェーズに移動 =========
            SceneManager.LoadScene("InvasionPhase");
        }
    }
}