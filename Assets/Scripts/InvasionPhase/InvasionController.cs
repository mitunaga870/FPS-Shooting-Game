using Enums;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace InvasionPhase
{
    public class InvasionController : MonoBehaviour
    {
        /** シーン間のデータ共有オブジェクト */
        [SerializeField] private CreateToInvasionData createToInvasionData;

        /** 迷路作成等を行うコントローラ */
        [SerializeField] private InvasionMazeController mazeController;

        /** 侵攻の制御を行うコントローラー */
        [FormerlySerializedAs("_invasionEnemyController")] [SerializeField]
        private InvasionEnemyController invasionEnemyController;

        /** ゲームの状態 */
        // ReSharper disable once MemberCanBePrivate.Global
        public GameState GameState { get; private set; } = GameState.BeforeStart;

        /** 高速時の倍速率 */
        // ReSharper disable once InconsistentNaming
        private const int FAST_SPEED = 2;

        /** ゲーム時間 */
        public int GameTime { get; private set; }

        private void FixedUpdate()
        {
            // 再生中ならゲーム時間を進める
            if (GameState == GameState.Playing)
            {
                GameTime++;
            }
            else if (GameState == GameState.FastPlaying)
            {
                GameTime += FAST_SPEED;
            }
        }

        // Start is called before the first frame update
        public void Start()
        {
            // セーブデータ読み込み
            var tileData = SaveController.LoadTileData();
            var trapData = SaveController.LoadTrapData();

            if (createToInvasionData.IsInvasion)
            {
                // シーン間のデータ共有オブジェクトからデータを取得
                mazeController.Create(createToInvasionData.TileData, createToInvasionData.TrapData);
                // 読み込み後はフラグを戻す
                createToInvasionData.IsInvasion = false;
            }
            else
            {
                // セーブデータからデータを取得
                mazeController.Create(tileData, trapData);
            }

            // 侵攻開始
            StartGame();
        }

        /**
         * やめるときの保存処理
         */
        private void OnApplicationQuit()
        {
            // セーブデータを保存
            SaveController.SavePhase(Phase.Invade);
            // シーン遷移で読み込んだデータをそのまま保存
            SaveController.SaveTileData(mazeController.TileData);
            SaveController.SaveTrapData(mazeController.TrapData);
        }


        /**
         * ゲーム開始メソッド
         */
        private void StartGame()
        {
            // ゲームの状態をプレイ中に変更
            GameState = GameState.Playing;

            // 各コントローラー
            invasionEnemyController.StartGame();
        }

        public void PauseGame()
        {
            GameState = GameState.Pause;
        }

        public void ResumeGame()
        {
            GameState = GameState.Playing;
        }

        public void ClearGame()
        {
            GameState = GameState.Clear;
        }

        public void FastPlay()
        {
            GameState = GameState.FastPlaying;
        }
    }
}