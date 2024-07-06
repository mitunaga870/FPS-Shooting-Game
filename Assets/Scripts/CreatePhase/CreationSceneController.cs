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
        [SerializeField] private MazeData stageData;

        /**
         * 侵攻フェーズに移動する
         */
        public void GoToInvasionPhase()
        {
            // ========= シーン間のデータ共有オブジェクト関連 =========
            // 迷路情報が正しいか確認
            if (!(
                    stageData.MazeColumns == createToInvasionData.GetMazeColumn() &&
                    stageData.MazeRows == createToInvasionData.GetMazeRow() &&
                    stageData.TrapCount == createToInvasionData.GetTrapCount()
                ))
            {
                return;
            }

            // ========= 侵攻フェーズに移動 =========
            SceneManager.LoadScene("InvasionPhase");
        }
    }
}