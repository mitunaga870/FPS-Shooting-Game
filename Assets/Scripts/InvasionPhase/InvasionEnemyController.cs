using System;
using DataClass;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionEnemyController : MonoBehaviour
    {
        [SerializeField] private GeneralS2SData _generalS2SData;

        [SerializeField] private StageObject _stageObject;

        [SerializeField] private InvasionController _invasionController;
        
        [SerializeField] private InvasionMazeController _invasionMazeController;

        /**
         * 現在のステージデータ
         */
        private StageData currentStageData;

        /**
         * 起動済みフラグ
         */
        private bool isAwaked = false;

        public void Start()
        {
            // 現在のステージ番号を取得
            var stage = _generalS2SData.Stage;
            var level = _generalS2SData.Level;

            // ステージデータを取得
            currentStageData = _stageObject.GetStageData(0);
        }

        public void Update()
        {
            // 未開始時は何もしない
            if (!isAwaked) return;
            
            // ゲーム自国の追加
            var time = _invasionController.GameTime;

            // 侵攻データを取得
            var invasionData = currentStageData.invasionData;
            // 生成データを取得
            var spawnData = invasionData.GetSpawnData(time);

            // 敵を沸かせる
            if (spawnData != null) SpawnEnemy(spawnData);
        }

        /**
         * 敵を沸かせる
         */
        private void SpawnEnemy(SpawnData spawnData)
        {
            // 敵を生成
            var enemy = Instantiate(spawnData.enemy);
            // 敵のスピードを設定
            enemy.Initialize(10, 10, _invasionMazeController.StartPosition, _invasionMazeController);
        }

        /**
         * スタート処理
         */
        public void StartGame()
        {
            // フラグをあげる
            isAwaked = true;
        }
    }
}