using System;
using DataClass;
using Enums;
using InvasionPhase;
using JetBrains.Annotations;
using UnityEngine;

namespace AClass
{
    public abstract class AEnemy : MonoBehaviour
    {
        // 0.02秒の重力加速度
        private const float Gravity = 9.8f * 0.02f;

        // 敵ごとのパラメータ、多分最終的には別のとこで管理する
        private int HP { get; set; }
        private int Attack { get; set; }

        /**
         * 移動速度、mTile/frame
         */
        private int Speed { get; set; }

        /** 自身の現在の目的地 */
        [CanBeNull]
        private TilePosition Destination { get; set; }

        /** 自身の現在地 */
        [CanBeNull]
        public TilePosition CurrentPosition { get; private set; }

        /** 現在の経路 */
        [CanBeNull]
        private Path Path { get; set; }

        /** 初期化済みフラグ */
        private bool Initialized { get; set; }

        /** Sceneコントローラー */
        private InvasionController _sceneController;

        /** 迷路コントローラー */
        private InvasionMazeController _mazeController;

        /** 現在の経路のインデックス */
        private int? CurrentPathIndex => Path?.Index(CurrentPosition);

        private InvasionEnemyController _enemyController;

        private int _prevTime;

        private EnemyStatus _status;

        // ============= ジャンプ用変数 =============
        private int _jumpDamage;
        private float _jumpSpeed;
        // ========================================

        // ============= ノックバック用変数 =============
        private const int _KnockBackSpeed = 10;
        private TilePosition _knockBackDestination;
        // ============================================

        // ============= ポイズン用変数 =============
        private bool _hasPoison = false;
        private int _poisonLevel;
        private int _poisonDamage;
        private int _poisonDuration = 0;
        // ============================================

        // ============= スロー用変数 =============
        private bool _hasSlow = false;
        private float _slowPercentage;
        private int _slowDuration;
        // ============================================

        /**
         * 初期化処理
         */
        private void Start()
        {
            Destination = _mazeController.GoalPosition;
        }

        /**
         * マイフレームの処理
         */
        private void FixedUpdate()
        {
            // 初期化されていない場合は何もしない
            if (!Initialized) return;

            // 初期化エラー確認
            if (CurrentPosition == null) throw new Exception("初期化処理に失敗しています");

            // 時刻を取得
            var time = _sceneController.GameTime;
            // 時刻差を取得
            var timeDiff = time - _prevTime;

            var mazeOrigin = _mazeController.MazeOrigin;

            // ポイズン処理
            if (_hasPoison)
            {
                // ダメージ計算
                // 効果時間が時間差分より小さい場合は効果時間を使う
                var damage = _poisonDamage * (timeDiff < _poisonDuration ? timeDiff : _poisonDuration);
                // ダメージ処理
                Damage((int)damage);
                // 残り時間を減らす
                _poisonDuration -= timeDiff;
                // 残り時間が0以下になった場合は解除
                if (_poisonDuration <= 0) _hasPoison = false;
            }

            // 燃焼床処理
            if (_mazeController.IsIgnite(CurrentPosition))
            {
                var tile = (InvasionPhaseTile)_mazeController.GetTile(CurrentPosition.Row, CurrentPosition.Col);
                if (tile == null) throw new Exception("Tile is null");

                // 発火情報を取得
                var igniteDamage = tile.IgniteDuration;
                var igniteDuration = tile.IgniteDuration;

                // ダメージ計算
                // 効果時間が時間差分より小さい場合は効果時間を使う
                var damage = igniteDamage * (timeDiff < igniteDuration ? timeDiff : igniteDuration);
                // ダメージ処理
                Damage((int)damage);

                Debug.Log("ignite Damage: " + damage);
            }

            switch (_status)
            {
                case EnemyStatus.None:
                    Moving(time, timeDiff, mazeOrigin);
                    break;
                case EnemyStatus.Jumping:
                    Jumping(time, timeDiff, mazeOrigin);
                    break;
                case EnemyStatus.KnockBacking:
                    KnockBacking(time, timeDiff, mazeOrigin);
                    break;
            }

            // ロード済み時間を更新
            _prevTime = time;
        }

        /**
         * 移動処理
         */
        // ReSharper disable once UnusedParameter.Local
        private void Moving(int time, int timeDiff, Vector3 mazeOrigin)
        {
            // 現在地がない場合はエラー
            if (CurrentPosition == null) throw new ArgumentNullException($"Cant find current position.");

            // 目的地がない場合はその場で待機
            if (Destination == null) return;

            // 経路がない場合は生成
            if (Path == null)
            {
                Path = _mazeController.GetShortestPath(CurrentPosition, Destination);

                // 位置をタイルの中心に補正
                var _localTransform = transform;
                var _position = _localTransform.position;
                _position = Path.Get(0).ToVector3(mazeOrigin);
                _localTransform.position = _position;
            }

            // 経路がない場合はエラー
            if (Path == null) throw new ArgumentNullException($"Cant find path to destination.");

            // 現在のパス番号がない場合はエラー
            var pathIndex = CurrentPathIndex ?? throw new ArgumentNullException($"Cant find current path index.");

            // 現在と目的地のタイル位置を取得
            var currentTileCoordinate = CurrentPosition.ToVector3(mazeOrigin);
            var nextTilePosition = Path.Get(pathIndex + 1);
            var nextTileCoordinate = nextTilePosition.ToVector3(mazeOrigin);

            var localTransform = transform;

            // 移動
            var moveAmount = (nextTileCoordinate - currentTileCoordinate) / 1000 * (Speed * timeDiff);
            // スロー状態の場合はスロー処理
            if (_hasSlow)
            {
                if (_slowDuration >= timeDiff) // 効果時間が時間差分より大きい場合
                {
                    moveAmount *= 1 - _slowPercentage;
                    _slowDuration -= timeDiff;
                }
                else // 効果時間が時間差分より小さい場合
                {
                    moveAmount *= 1 - _slowPercentage;
                    moveAmount *= _slowDuration / (float)timeDiff;
                    _slowDuration = 0;
                    _hasSlow = false;
                }
            }

            var position = localTransform.position;
            position += moveAmount;
            localTransform.position = position;

            // 次のタイルとの距離
            var distance = Vector3.Distance(position, nextTileCoordinate);
            // 次のタイルに到達した場合
            if (distance < 0.1f)
                // 現在地を更新
                CurrentPosition = nextTilePosition;

            if (CurrentPosition == null) throw new Exception("Current position is null");

            // トラップに引っかかった場合
            _mazeController.AwakeTrap(CurrentPosition);

            // 目的地に到達した場合
            if (CurrentPosition.Equals(Destination))
            {
                Destination = null;
                Path = null;
            }

            // ゴールに到達した場合
            if (CurrentPosition.Equals(_mazeController.GoalPosition))
            {
                _sceneController.EnterEnemy(Attack);

                // ゲームオブジェクトを削除
                Destroy(gameObject);
            }
        }

        /**
         * ジャンプ中処理
         */
        private void Jumping(
            // ReSharper disable once UnusedParameter.Local
            int time,
            int timeDiff,
            Vector3 mazeOrigin
        )
        {
            // 移動
            var localTransform = transform;
            var position = localTransform.position;
            position += new Vector3(0, _jumpSpeed, 0);
            localTransform.position = position;

            // 現在地を更新
            CurrentPosition = new TilePosition(position, mazeOrigin);
            
            // 速度を減速
            _jumpSpeed -= Gravity * timeDiff;

            // 地面に着地した場合
            if (position.y <= 0)
            {
                // ジャンプ中フラグを解除
                _status = EnemyStatus.None;

                // ダメージ処理
                Damage(_jumpDamage);
            }
        }

        /**
         * ノックバック中処理
         */
        private void KnockBacking(
            int time,
            int timeDiff,
            Vector3 mazeOrigin)
        {
            // 現在と目的地のタイル位置を取得
            var startTileCoordinate = CurrentPosition.ToVector3(mazeOrigin);
            var destinationPosition = _knockBackDestination;
            var nextTileCoordinate = destinationPosition.ToVector3(mazeOrigin);

            // 目的地まで等速で移動
            var moveAmount = (nextTileCoordinate - startTileCoordinate) / 1000 * (_KnockBackSpeed * timeDiff);

            var localTransform = transform;

            // 移動
            var position = localTransform.position;
            position += moveAmount;
            localTransform.position = position;

            // 到着した場合
            if (Vector3.Distance(position, nextTileCoordinate) < 0.1f)
            {
                // 現在地を更新
                CurrentPosition = destinationPosition;
                // ノックバック中フラグを解除
                _status = EnemyStatus.None;
            }
        }

        /**
         * ダメージ処理
         */
        public void Damage(int damage)
        {
            HP -= damage;

            // HPが0以下になった場合
            if (HP <= 0)
                // ゲームオブジェクトを削除
                Destroy(gameObject);
        }

        /**
         * 初期化処理
         * @param hp HP
         * @param speed 移動速度 mTile/frame
         * @param startPosition 初期位置
         */
        public void Initialize(int hp, int speed, int attack, TilePosition startPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController)
        {
            // 初期化済みはエラー
            if (Initialized) throw new Exception("This enemy is already initialized.");

            // 座標に変換
            var position = startPosition.ToVector3(mazeController.MazeOrigin);
            // 初期ポイントに移動
            transform.position = position;

            // 初期情報登録
            CurrentPosition = startPosition;
            HP = hp;
            Speed = speed;
            Attack = attack;
            _sceneController = sceneController;
            _mazeController = mazeController;
            _enemyController = enemyController;

            Initialized = true;
        }

        private void OnDestroy()
        {
            // 敵の削除処理
            _enemyController.EnemyDestroyed(GetInstanceID());
        }

        public void Jump(int height, int damage)
        {
            // ジャンプ中は何もしない
            if (_status != EnemyStatus.None) return;

            // ジャンプ中フラグを立てる
            _status = EnemyStatus.Jumping;

            // ジャンプ情報を登録
            _jumpDamage = damage;

            // ジャンプ速度を計算
            _jumpSpeed = Mathf.Sqrt(2 * Gravity * height);
        }

        public void KnockBack(int distance)
        {
            // knockバック中は何もしない
            if (_status != EnemyStatus.None) return;

            // ノックバック中フラグを立てる
            _status = EnemyStatus.KnockBacking;

            // 現在パスがない場合はエラー
            if (Path == null) throw new Exception("Current path index is null");
            // 現在地がない場合はエラー
            if (CurrentPosition == null) throw new Exception("Current position is null");
            if (CurrentPathIndex == null) throw new Exception("Current path index is null");

            // ノックバック距離よりもスタートの方が小さい場合は上書き
            if (CurrentPathIndex.Value < distance)
                distance = CurrentPathIndex.Value;
            
            // knockバック先を初期化
            _knockBackDestination = CurrentPosition;

            // 現在のパスから一直線で戻れる位置を計算
            while (distance > 0)
            {
                // いったん距離分戻す
                var tmpDestination = Path.Get(
                    CurrentPathIndex.Value - distance
                );
                
                // 距離を減らす
                distance--;

                if (tmpDestination == null) break;

                // 一直線に戻れるか確認
                if (CurrentPosition.Col != tmpDestination.Col && CurrentPosition.Row != tmpDestination.Row) continue;

                // 一直線に戻れる場合は目的地を更新
                _knockBackDestination = tmpDestination;
            }
        }

        public void InfusePoison(int damage, int duration, int level)
        {
            // すでに毒状態の場合は強い方を採用
            if (_hasPoison && _poisonLevel < level)
                return;
            // 同レベルの場合は時間を延長
            if (_hasPoison && _poisonLevel == level)
                _poisonDuration += duration;

            _hasPoison = true;
            _poisonLevel = level;
            _poisonDamage = damage;
            _poisonDuration = duration;
        }

        /**
         * 目的地を設定
         */
        public void SetDestination(TilePosition setPosition)
        {
            // 目的地を設定
            Destination = setPosition;
            // 現在ルートを消す
            Path = null;
        }

        /**
         * 目的地を解除
         */
        public void ReleaseDestination()
        {
            // 目的地を解除
            Destination = _mazeController.GoalPosition;
            // 現在ルートを消す
            Path = null;
        }

        public void Slow(float slowPercentage, int duration)
        {
            // すでにスロー状態の場合は強い方を採用
            if (_hasSlow && _slowPercentage < slowPercentage)
                return;
            // 同レベルの場合は時間を延長
            if (_hasSlow && Math.Abs(_slowPercentage - slowPercentage) < 0.01)
                _slowDuration += duration;

            _hasSlow = true;
            _slowPercentage = slowPercentage;
            _slowDuration = duration;
        }
    }
}