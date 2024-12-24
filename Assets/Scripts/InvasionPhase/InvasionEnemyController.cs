using System.Collections.Generic;
using System.Linq;
using AClass;
using DataClass;
using JetBrains.Annotations;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace InvasionPhase
{
    public class InvasionEnemyController : MonoBehaviour
    {
        [FormerlySerializedAs("_invasionController")]
        [SerializeField]
        private InvasionController invasionController;

        [FormerlySerializedAs("_invasionMazeController")]
        [SerializeField]
        private InvasionMazeController invasionMazeController;
        
        [SerializeField]
        private GeneralS2SData generalS2SData;

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

        /** 現ステージの侵攻データ */
        public InvasionData CurrentInvasionData => _currentStageData.invasionData;
        
        /** 敵の現在地リスト　*/
        private List<TilePosition> _enemyTiles = new();

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
            
            // 時間が更新されていたらトラップを発火してリストをリフレッシュ
            if (diff > 0) 
            {
                // 敵がいるタイルリストのトラップを発火
                foreach (var tile in _enemyTiles)
                {
                    invasionMazeController.AwakeTrap(tile);
                }
                
                // 敵がいるタイルリストをリフレッシュ
                _enemyTiles.Clear();
            }
            
            // 敵リストを走査
            foreach (var enemy in _enemies)
            {
                // 敵の位置を取得
                var position = enemy.CurrentPosition;
                // 位置がnullでない場合はリストに追加
                if (position == null) continue;
                // 追加されているか確認
                if (!_enemyTiles.Contains(position)) _enemyTiles.Add(position);
            }
            
        }

        /**
         * 敵を沸かせる
         */
        private void SpawnEnemy(SpawnData spawnData)
        {
            // スケーリングの倍率計算
            // 参照値にかける倍率を指定
            var factorMapNum = 1f;
            var factorColNum = 0.05f;
            var mapNum = generalS2SData.MapNumber;
            var colNum = generalS2SData.CurrentMapColumn;
            var scale = 1 + mapNum * factorMapNum + colNum * factorColNum;
            
            // 値をスケーリング
            var health = (int) (invasionMazeController.StageData.enemyHp * scale);
            var attack = (int) (1 * scale); 
            
            for (var i = 0; i < spawnData.spawnCount; i++)
            {
                // 敵を生成
                var enemy = Instantiate(spawnData.enemy);
                
                // 敵のスピードを設定
                enemy.Initialize(
                    health,
                    10, attack,
                    invasionController.GameTime,
                    invasionMazeController.StartPosition,
                    invasionController,
                    invasionMazeController,
                    this);

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
                }
        }

        [ItemCanBeNull]
        public List<AEnemy> GetEnemies([CanBeNull] TilePosition[] effectAreas)
        {
            var result = new List<AEnemy>();

            // エリアが指定されていない場合は全ての敵を返す
            if (effectAreas == null)
            {
                result.AddRange(_enemies);
                return result;
            }

            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらリストに追加
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
                }
        }

        public void KnockBackEnemy(TilePosition position, int distance, int stunTime = -1)
        {
            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらダメージを与える
                if (enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(position))
                {
                    enemy.KnockBack(distance, stunTime);
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
                }
        }

        public void FlipEnemy(TilePosition position, int damage, int duration)
        {
            // 敵リストを走査
            foreach (var enemy in _enemies)
                // 位置が一致したらダメージを与えペラペラにする
                if (enemy.CurrentPosition != null && enemy.CurrentPosition.Equals(position))
                {
                    enemy.Damage(damage);
                    enemy.Stun(duration);
                }
        }

        public void ReCalculationPath()
        {
            // 敵リストを走査
            foreach (var enemy in _enemies) enemy.ReCalculationPath();
        }
    }
}