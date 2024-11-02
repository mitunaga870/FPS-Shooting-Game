using System.Collections.Generic;
using System.Linq;
using AClass;
using DataClass;
using JetBrains.Annotations;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace InvasionPhase
{
    public class InvasionEnemyController : MonoBehaviour
    {
        [FormerlySerializedAs("_generalS2SData")]
        [SerializeField]
#pragma warning disable CS0414 // フィールドは割り当てられていますがその値は使用されていません
        private GeneralS2SData generalS2SData;
#pragma warning restore CS0414 // フィールドは割り当てられていますがその値は使用されていません

        [FormerlySerializedAs("_stageObject")]
        [SerializeField]
        private StageObject stageObject;

        [FormerlySerializedAs("_invasionController")]
        [SerializeField]
        private InvasionController invasionController;

        [FormerlySerializedAs("_invasionMazeController")]
        [SerializeField]
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

        /**
         * 管理している敵のリスト
         */
        private List<AEnemy> _enemies = new();

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
                if (spawnData != null) SpawnEnemy(spawnData);

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
                enemy.Initialize(10, 10, 1, invasionMazeController.StartPosition, invasionController,
                    invasionMazeController, this);

                // 敵をリストに追加
                _enemies.Add(enemy);
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

        public void EnemyDestroyed(int id)
        {
            // 残りの敵数を減らす
            _remainingEnemyCount--;

            // 敵リストを走査
            foreach (var enemy in _enemies)
                // IDが一致したらリストから削除
                if (enemy.GetInstanceID() == id)
                {
                    _enemies.Remove(enemy);
                    break;
                }


            // 残りの敵数が0になったらゲーム終了
            if (_remainingEnemyCount == 0) invasionController.ClearGame();
        }

        /**
         * 指定タイルの敵にダメージを与える
         */
        public void DamageEnemy(TilePosition position, int i)
        {
            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらダメージを与える
                if (enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(position))
                {
                    enemy.Damage(i);
                    break;
                }
        }

        [ItemCanBeNull]
        public List<AEnemy> GetEnemies(TilePosition[] effectAreas)
        {
            var result = new List<AEnemy>();

            foreach (var enemy in _enemies)
                result.AddRange(from effectArea in effectAreas
                    where enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(effectArea)
                    select enemy);

            return result;
        }

        /**
         * 指定タイルの敵を跳ね飛ばす
         */
        public void JumpEnemy(TilePosition position, int height, int damage)
        {
            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらダメージを与える
                if (enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(position))
                {
                    enemy.Jump(height, damage);
                    break;
                }
        }

        public void KnockBackEnemy(TilePosition position, int distance)
        {
            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらダメージを与える
                if (enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(position))
                {
                    enemy.KnockBack(distance);
                    break;
                }
        }

        public void InfusePoison(TilePosition position, int damage, int duration, int level)
        {
            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらダメージを与える
                if (enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(position))
                {
                    enemy.InfusePoison(damage, duration, level);
                    break;
                }
        }
    }
}