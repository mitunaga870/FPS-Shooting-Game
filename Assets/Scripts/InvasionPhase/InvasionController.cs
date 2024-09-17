using System.Threading.Tasks;
using AClass;
using DataClass;
using Enums;
using lib;
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

        /** セレクト状態の減速率（1/nの値になってないとバグるかも）*/
        // ReSharper disable once InconsistentNaming
        private const float SELECTING_SPEED = 0.5f;

        /** 減速時の時刻スタック（１を超えたら０にして時刻を進める） */
        private float delayTimeStack = 0;

        /** ゲーム時間 */
        public int GameTime { get; private set; }

        /** プレイヤーHP */
        public int PlayerHp { get; private set; }

        private void FixedUpdate()
        {
            // 再生中ならゲーム時間を進める
            switch(GameState)
            {
                case GameState.Playing:
                    // 通常再生
                    GameTime++;
                    break;
                case GameState.FastPlaying:
                    // 高速再生
                    GameTime += FAST_SPEED;
                    break;
                case GameState.Selecting:
                    // 選択状態
                    delayTimeStack += SELECTING_SPEED;
                    if (delayTimeStack >= 1)
                    {
                        GameTime++;
                        delayTimeStack = 0;
                    }
                    break;
                    
            }
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

            // プレイヤーデータ読み込み
            PlayerHp = GeneralS2SData.PlayerHp;

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
            // ゲームオーバー時はクリアしない
            if (GameState == GameState.GameOver) return;

            Debug.Log("Game Clear!");
            GameState = GameState.Clear;
        }

        public void FastPlay()
        {
            GameState = GameState.FastPlaying;
        }

        public void EnterEnemy(int damage)
        {
            PlayerHp -= damage;
            if (PlayerHp <= 0)
            {
                Debug.Log("Game Over!");
                GameState = GameState.GameOver;
            }
        }

        public async void SetSkillMode(ASkill _skill)
        {
            this.GameState = GameState.Selecting;
        }
    }
}