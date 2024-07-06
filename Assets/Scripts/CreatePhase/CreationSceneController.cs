using ScriptableObjects.S2SDataObjects;
using UnityEngine;

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

        /**
         * 侵攻フェーズに移動する
         */
        public void GoToInvasionPhase()
        {
            // ========= シーン間のデータ共有オブジェクト関連 =========
            // 迷路情報が正しいか確認
            Debug.Log(createToInvasionData);
        }
    }
}