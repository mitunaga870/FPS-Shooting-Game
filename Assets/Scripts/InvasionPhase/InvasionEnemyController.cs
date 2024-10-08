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
#pragma warning disable CS0414 // フィールドは割り当てられていますがその値は使用されていません
        private GeneralS2SData generalS2SData;
#pragma warning restore CS0414 // フィールドは割り当てられていますがその値は使用されていません

        [FormerlySerializedAs("_stageObject")] [SerializeField]
        private StageObject stageObject;

        [FormerlySerializedAs("_invasionController")] [SerializeField]
        private InvasionController invasionController;

        [FormerlySerializedAs("_invasionMazeController")] [SerializeField]
        private InvasionMazeController invasionMazeController;

        /**
         * 残りの敵数
         */
        private int _remainingEnemyCount;

        /**
         * 現在のステージデータ
         */
        private StageData _currentStageData;

        /**
         * 読み込み済み時間
         */
        private int _prevTime;

        /**
         * 起動済みフラグ
         */
        private bool _isAwoke;

        public void Start()
        {
            // ステージデータを取得
            _currentStageData = invasionMazeController.StageData;

            // 残りの敵数を設定
            _remainingEnemyCount = _currentStageData.invasionData.GetEnemyCount();
        }

        public void Update()
        {
            // 未開始時は何もしない
            if (!_isAwoke) return;

            // ゲーム自国の追加
            var time = invasionController.GameTime;

            // 時間が進んでいない場合は何もしない
            if (time <= _prevTime) return;

            // 時間差分を取得
            var diff = time - _prevTime;

            // 時間差分分だけスポーンデータをかくにん
            for (var i = 0; i < diff;)
            {
                // 侵攻データを取得
                var invasionData = _currentStageData.invasionData;
                // 生成データを取得
                var spawnData = invasionData.GetSpawnData(time - i);

                // 敵を沸かせる
                if (spawnData != null)
                {
                    SpawnEnemy(spawnData);
                }

                i++;
            }

            // 読み込み済み時間を更新
            _prevTime = time;
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
            _isAwoke = true;
        }

        public void EnemyDestroyed()
        {
            // 残りの敵数を減らす
            _remainingEnemyCount--;


            // 残りの敵数が0になったらゲーム終了
            if (_remainingEnemyCount == 0)
            {
                invasionController.ClearGame();
            }
        }
    }
}