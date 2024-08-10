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
            var enemy = Instantiate(
                _testEnemy,
                mazeController.StartPosition.ToVector3(createToInvasionData.MazeOrigin),
                Quaternion.identity
            );
            enemy.Initialize(10, 10, mazeController.StartPosition, mazeController);
        }

        /**
         * ゲーム開始メソッド
         */
        public void StartGame()
        {
            // ゲームの状態をプレイ中に変更
            _gameState = GameState.Playing;
        }
    }
}