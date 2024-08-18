using System;
using DataClass;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace InvasionPhase
{
    public class InvasionEnemyController : MonoBehaviour
    {
        [FormerlySerializedAs("_generalS2SData")] [SerializeField]
        private GeneralS2SData generalS2SData;

        [FormerlySerializedAs("_stageObject")] [SerializeField]
        private StageObject stageObject;

        [FormerlySerializedAs("_invasionController")] [SerializeField]
        private InvasionController invasionController;

        [FormerlySerializedAs("_invasionMazeController")] [SerializeField]
        private InvasionMazeController invasionMazeController;

        /**
         * 残りの敵数
         */
        private int remainingEnemyCount;

        /**
         * 現在のステージデータ
         */
        private StageData currentStageData;

        /**
         * 読み込み済み時間
         */
        private int prevTime = 0;

        /**
         * 起動済みフラグ
         */
        private bool isAwaked = false;

        public void Start()
        {
            // 現在のステージ番号を取得
            var stage = generalS2SData.Stage;
            var level = generalS2SData.Level;

            // ステージデータを取得
            currentStageData = stageObject.GetStageData(0);

            // 残りの敵数を設定
            remainingEnemyCount = currentStageData.invasionData.GetEnemyCount();
        }

        public void Update()
        {
            // 未開始時は何もしない
            if (!isAwaked) return;

            // ゲーム自国の追加
            var time = invasionController.GameTime;

            // 時間が進んでいない場合は何もしない
            if (time <= prevTime) return;

            // 時間差分を取得
            var diff = time - prevTime;

            // 時間差分分だけスポーンデータをかくにん
            for (var i = 0; i < diff;)
            {
                // 侵攻データを取得
                var invasionData = currentStageData.invasionData;
                // 生成データを取得
                var spawnData = invasionData.GetSpawnData(time - i);

                // 敵を沸かせる
                if (spawnData != null)
                {
                    Debug.Log("spawn enemy");
                    SpawnEnemy(spawnData);
                }

                i++;
            }

            // 読み込み済み時間を更新
            prevTime = time;
        }

        /**
         * 敵を沸かせる
         */
        private void SpawnEnemy(SpawnData spawnData)
        {
            for (var i = 0; i < spawnData.spawnCount; i++)
            {
                // 敵を生成
                var enemy = Instantiate(spawnData.enemy);
                // 敵のスピードを設定
                enemy.Initialize(10, 10, invasionMazeController.StartPosition, invasionController,
                    invasionMazeController, this);
            }
        }

        /**
         * スタート処理
         */
        public void StartGame()
        {
            // フラグをあげる
            isAwaked = true;
        }

        public void EnemyDestroyed()
        {
            // 残りの敵数を減らす
            remainingEnemyCount--;


            // 残りの敵数が0になったらゲーム終了
            if (remainingEnemyCount == 0)
            {
                invasionController.ClearGame();
            }
        }
    }
}