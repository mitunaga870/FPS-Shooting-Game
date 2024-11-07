using System;
using AClass;
using DataClass;
using Enums;
using Map;
using Map.UI;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using Shop;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace InvasionPhase
{
    public class InvasionController : MonoBehaviour
    {
        /**
         * 高速時の倍速率
         */
        // ReSharper disable once InconsistentNaming
        private const int FAST_SPEED = 2;

        /** セレクト状態の減速率（1/nの値になってないとバグるかも）*/
        // ReSharper disable once InconsistentNaming
        private const float SELECTING_SPEED = 0.5f;

        /**
         * シーン間のデータ共有オブジェクト
         */
        [SerializeField]
        private CreateToInvasionData createToInvasionData;

        /**
         * 迷路作成等を行うコントローラ
         */
        [SerializeField]
        private InvasionMazeController mazeController;

        /**
         * ステージデータを読み込むためのオブジェクト
         */
        [SerializeField]
        private StageObject stageObject;

        /**
         * 侵攻の制御を行うコントローラー
         */
        [FormerlySerializedAs("_invasionEnemyController")]
        [SerializeField]
        private InvasionEnemyController invasionEnemyController;

        /**
         * 財布
         */
        [SerializeField]
        private WalletController walletController;

        [SerializeField]
        private GeneralS2SData generalS2SData;
        
        // スキルのUI
        [SerializeField]
        private GameObject skillUI;

        /**
         * デッキ
         */
        [SerializeField]
        private DeckController deckController;
        
        [SerializeField]
        private MapController mapController;
        
        // =============== ショップ系 =====================
        [SerializeField]
        private ShopController _shopUI;

        /**
         * 減速時の時刻スタック（１を超えたら０にして時刻を進める）
         */
        private float delayTimeStack;

        /**
         * ゲームの状態
         */
        public GameState GameState { get; private set; } = GameState.BeforeStart;

        /**
         * ゲーム時間
         */
        public int GameTime { get; private set; }
        
        /**
         * スロー用のバッファ
         */
        private int _slowBuffer = 0;
        
        /**
         * スロー再生時の減速率
         */
        private const int SLOW_SPEED = 2;

        /**
         * プレイヤーHP
         */
        public int PlayerHp { get; private set; }

        /**
         * ステージデータ
         */
        public StageData StageData { get; private set; }
        
        /**
         * ゲーム終了時のフラグ
         */
        private bool _isApplicateQuit = false;
        
        // =============== スキル用変数 =====================
        private bool _isSkillMode;
        [NonSerialized]
        public ASkill Skill;
        // =================================================
        
        // Start is called before the first frame update
        public void Start()
        {
            // セーブデータ読み込み
            var tileData = SaveController.LoadTileData();
            var trapData = SaveController.LoadTrapData();
            var turretData = SaveController.LoadTurretData();
            var openShop = SaveController.LoadShopFlag();

            // ステージデータ読み込み
            StageData = mazeController.StageData;
            
            // スキル禁止処理
            if (!StageData.StageCustomData.IsAllowedToUseSkill)
            {
                // スキルを使えないようにする
                skillUI.SetActive(false);
            }
            
            // ショップならショップを開いて終わり
            if (openShop)
            {
                // 次に遷移する変わりにショップを開く
                var shop = Instantiate(_shopUI);
                shop.Initialize(deckController, walletController);
                
                // ショップが閉じられた時の処理を追加
                shop.SetOnClose(() =>
                {
                    // マップを開く
                    mapController.ShowMap(false, true);
                });
                
                return;
            }

            if (createToInvasionData.IsInvasion)
            {
                // シーン間のデータ共有オブジェクトからデータを取得
                mazeController.Create(
                    createToInvasionData.TileData,
                    createToInvasionData.TrapData,
                    createToInvasionData.TurretData
                );
                // 読み込み後はフラグを戻す
                createToInvasionData.IsInvasion = false;
            }
            else
            {
                // セーブデータからデータを取得
                mazeController.Create(tileData, trapData, turretData);
            }

            // プレイヤーデータ読み込み
            PlayerHp = generalS2SData.PlayerHp;

            // 侵攻開始
            StartGame();
        }

        private void FixedUpdate()
        {
            // 再生中ならゲーム時間を進める
            switch (GameState)
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
                    
                    // 右クリックでキャンセル
                    if (Input.GetMouseButtonDown(1))
                    {
                        _isSkillMode = false;
                        GameState = GameState.Playing;
                    }

                    break;
            }

            if (GameState == GameState.Playing)
                GameTime++;
            else if (GameState == GameState.FastPlaying) GameTime += FAST_SPEED;
        }

        /**
         * やめるときの保存処理
         */
        private void OnApplicationQuit()
        {
            // ゲーム終了時のフラグを立てる
            _isApplicateQuit = true;
            
            // セーブデータを保存
            SaveController.SavePhase(Phase.Invade);
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
            
            // アプリケーション終了処理中はクリアしない
            if (_isApplicateQuit) return;

            Debug.Log("Game Clear!");
            GameState = GameState.Clear;

            // 報酬付与
            var reward = StageData.GetReward(stageObject);

            // TODO: ここで報酬を付与するUIを出したい　とりあえず即時付与
            // お金
            walletController.AddWallet(reward.money);
            // トラップ
            for (var i = 0; i < reward.randomTrap; i++)
            {
                // ランダムなトラップを取得
                var all = Resources.LoadAll<ATrap>("Prefabs/Traps");
                var trap = all[Random.Range(0, all.Length)];

                // デッキに追加
                deckController.AddTrap(trap);
            }

            // 指定トラップ
            var selectedTrap = reward.selectedTrap;
            deckController.AddTrapRange(selectedTrap);

            // タレット
            for (var i = 0; i < reward.randomTurret; i++)
            {
                // ランダムなタレットを取得
                var all = Resources.LoadAll<ATurret>("Prefabs/Turrets");
                var turret = all[Random.Range(0, all.Length)];

                // デッキに追加
                deckController.AddTurret(turret);
            }

            // 指定タレット
            var selectedTurret = reward.selectedTurret;

            // スキル
            for (var i = 0; i < reward.randomSkill; i++)
            {
                // ランダムなスキルを取得
                var all = Resources.LoadAll<ASkill>("Prefabs/Skills");
                var skill = all[Random.Range(0, all.Length)];

                // デッキに追加
                deckController.AddSkill(skill);
            }

            // 指定スキル
            var selectedSkill = reward.selectedSkill;
            deckController.AddSkillRange(selectedSkill);

            if (StageData.StageType == StageType.Boss)
            {
                // ボスの時はステージを進める
                // 最終マップか確認 最大値は3
                if (generalS2SData.MapNumber == 3)
                {
                    // 最終マップならクリア
                    Debug.Log("All Clear!");
                    return;
                }

                // マップを進める
                generalS2SData.MapNumber++;
                // ポジション
                generalS2SData.CurrentMapRow = 0;
                generalS2SData.CurrentMapColumn = 0;
                // 迷路データをリセット
                createToInvasionData.Reset();

                // 作成フェーズに移行    
                SceneManager.LoadScene("CreatePhase");
            }
            else
            {
                mapController.ShowMap( false, true);
            }
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

        public void SetSkillMode(ASkill skill)
        {
            GameState = GameState.Selecting;
            
            Skill = skill;
            _isSkillMode = true;
        }
        
        public void UseSkill(TilePosition position)
        {
            if (!_isSkillMode) return;
            
            mazeController.HideEffectRange();
            
            Skill.UseSkill(
                position, 
                this,
                mazeController,
                invasionEnemyController
            );
            
            _isSkillMode = false;
            GameState = GameState.Playing;
        }
    }
}