using System;
using DataClass;
using Enemies;
using Enums;
using JetBrains.Annotations;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionController : MonoBehaviour
    {
        /** シーン間のデータ共有オブジェクト */
        [SerializeField] private CreateToInvasionData createToInvasionData;

        /** 迷路作成等を行うコントローラ */
        [SerializeField] private InvasionMazeController mazeController;

        /** 侵攻の制御を行うコントローラー */
        [SerializeField] private InvasionEnemyController _invasionEnemyController;

        /** 敵のプレファブ */
        [SerializeField] private TestEnemy _testEnemy;

        /** ゲームの状態 */
        public GameState GameState { get; private set; } = GameState.BeforeStart;

        /** 高速時の倍速率 */
        private const int FAST_SPEED = 2;

        /** ゲーム時間 */
        public int GameTime { get; private set; } = 0;

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
            mazeController.Create(createToInvasionData.TileData, createToInvasionData.TrapData);

            // 侵攻開始
            StartGame();
        }

        /**
         * ゲーム開始メソッド
         */
        public void StartGame()
        {
            // ゲームの状態をプレイ中に変更
            GameState = GameState.Playing;

            // 各コントローラー
            _invasionEnemyController.StartGame();
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