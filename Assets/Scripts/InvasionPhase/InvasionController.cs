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
        private Enums.GameState _gameState = GameState.BeforeStart;

        /** ゲーム時間 */
        public int GameTime { get; private set; } = 0;

        private void FixedUpdate()
        {
            // 再生中ならゲーム時間を進める
            if (_gameState == GameState.Playing)
            {
                GameTime++;
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
            _gameState = GameState.Playing;
            
            // 各コントローラー
            _invasionEnemyController.StartGame();
        }
    }
}