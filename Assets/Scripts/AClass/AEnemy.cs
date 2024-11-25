using System;
using DataClass;
using Enums;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

namespace AClass
{
    public abstract class AEnemy : MonoBehaviour
    {
        [SerializeField]
        GeneralS2SData generalS2SData;
        
        // 0.02秒の重力加速度
        private const float Gravity = 9.8f * 0.02f;

        // 敵ごとのパラメータ、多分最終的には別のとこで管理する
        private int HP { get; set; }
        private int Attack { get; set; }
        private int MaxHP { get; set; }
        private int RemainingLives { get; set; }

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
        protected InvasionController SceneController;

        /** 迷路コントローラー */
        private InvasionMazeController _mazeController;

        /** 現在の経路のインデックス */
        private int? CurrentPathIndex => Path?.Index(CurrentPosition);
        
        /** 死んでるか */
        private bool IsDead => HP <= 0;

        private InvasionEnemyController _enemyController;

        private int _prevTime;

        private EnemyCCStatus _ccStatus;

        // ============= ジャンプ用変数 =============
        private int _jumpDamage;
        private float _jumpSpeed;
        // ========================================

        // ============= ノックバック用変数 =============
        private const int KnockBackSpeed = 10;
        private TilePosition _knockBackDestination;
        private int _knockBackStunTime;
        // ============================================

        // ============= ポイズン用変数 =============
        private bool _hasPoison;
        private int _poisonLevel;
        private int _poisonDamage;
        private int _poisonDuration;
        // ============================================

        // ============= スロー用変数 =============
        private bool _hasSlow;
        private float _slowPercentage;
        private int _slowDuration;
        // ============================================
        
        // ============= ペラペラ用変数 =============
        private int _stunDuration;
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
        protected void FixedUpdate()
        {
            // 初期化されていない場合は何もしない
            if (!Initialized) return;

            // 初期化エラー確認
            if (CurrentPosition == null) throw new Exception("初期化処理に失敗しています");
            
            // 死んでいる場合は何もしない
            if (IsDead) return;

            // 時刻を取得
            var time = SceneController.GameTime;
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
                Damage(damage);
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
                var igniteDamage = tile.IgniteDamage;
                var igniteDuration = tile.IgniteDuration;

                // ダメージ計算
                // 効果時間が時間差分より小さい場合は効果時間を使う
                var damage = igniteDamage * (timeDiff < igniteDuration ? timeDiff : igniteDuration);
                // ダメージ処理
                Damage(damage);
            }
            
            // テレポート処理
            if (_mazeController.IsTeleport(CurrentPosition))
            {
                var tile = (InvasionPhaseTile)_mazeController.GetTile(CurrentPosition.Row, CurrentPosition.Col);
                if (tile == null) throw new Exception("Tile is null");

                // テレポート先を取得
                var teleportDestination = tile.WarpHoleDestination;
                // テレポート先がない場合はエラー
                if (teleportDestination == null) throw new Exception("Teleport destination is null");

                // テレポート先に移動
                transform.position = teleportDestination.ToVector3(mazeOrigin);
                // 現在地を更新
                CurrentPosition = teleportDestination;
                
                // ルートを消去
                Path = null;
            }
            
            // ノックバック床処理
            if (_mazeController.IsKnockBack(CurrentPosition))
            {
                var tile = (InvasionPhaseTile)_mazeController.GetTile(CurrentPosition.Row, CurrentPosition.Col);
                if (tile == null) throw new Exception("Tile is null");

                // ノックバック情報を取得
                var knockBackDistance = tile.KnockBackDistance;
                var knockBackStunTime = tile.KnockBackStunTime;

                // ノックバック処理
                KnockBack(knockBackDistance, knockBackStunTime);
                
                // ノックバック床を解除
                tile.ReleaseKnockBack();
            }

            switch (_ccStatus)
            {
                case EnemyCCStatus.None:
                    Moving(time, timeDiff, mazeOrigin);
                    break;
                case EnemyCCStatus.Jumping:
                    Jumping(time, timeDiff, mazeOrigin);
                    break;
                case EnemyCCStatus.KnockBacking:
                    KnockBacking(timeDiff, mazeOrigin);
                    break;
                case EnemyCCStatus.Stun:
                    Flipping(timeDiff);
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
            if (Destination == null)
            {
                PlayIdleAnimation();
                return;
            }
            
            // 移動中のアニメーションを再生
            PlayMoveAnimation();

            // 経路がない場合は生成
            if (Path == null)
            {
                Path = _mazeController.GetShortestPath(CurrentPosition, Destination);
                
                // 経緯がないときはエラー
                if (Path == null) throw new ArgumentNullException($"Cant find path from {CurrentPosition} to {Destination}");

                // 位置をタイルの中心に補正
                // ReSharper disable once InconsistentNaming
                var _localTransform = transform;
                // ReSharper disable once InconsistentNaming
                var _position = Path.Get(0).ToVector3(mazeOrigin);
                _position.y = GetHeight();
                _localTransform.position = _position;
            }

            // 現在のパス番号がない場合はエラー
            var pathIndex = CurrentPathIndex ?? throw new ArgumentNullException($"Cant find current path index.");

            // 現在と目的地のタイル位置を取得
            var currentTileCoordinate = CurrentPosition.ToVector3(mazeOrigin);
            var nextTilePosition = Path.Get(pathIndex + 1);
            var nextTileCoordinate = nextTilePosition.ToVector3(mazeOrigin);
            
            // 次のタイルがブロックエリアの時は今のタイルにとどまらせる
            if (_mazeController.GetTile(nextTilePosition)!.IsBlockArea)
            {
                // 現在のタイル位置に強制移動
                transform.position = currentTileCoordinate;
                
                // 移動させない
                return;
            }

            var localTransform = transform;
            
            // 向きを取得
            var direction = nextTileCoordinate - currentTileCoordinate;
            direction.y = 0;
            localTransform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);

            // 移動
            var moveAmount = (nextTileCoordinate - currentTileCoordinate) / 1000 * (Speed * timeDiff);
            // ステージデータで移動量をスケール
            moveAmount *= SceneController.StageData.StageCustomData.MoveSpeedScale;
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
            // 鈍化タイル上の場合はスロー処理
            if (_mazeController.IsSlow(CurrentPosition))
            {
                // 対象タイルを取得
                var tile = (InvasionPhaseTile)_mazeController.GetTile(CurrentPosition.Row, CurrentPosition.Col);

                if (tile != null) moveAmount *= 1 - tile.SlowAreaPower;
            }
            

            var position = localTransform.position;
            position += moveAmount;
            localTransform.position = position;

            // 次のタイルとの距離
            var distance = Vector3.Distance(position, nextTileCoordinate);
            // 次のタイルに到達した場合
            if (distance < Math.Sqrt(Math.Pow(0.1f, 2) + Math.Pow(GetHeight(), 2)))
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
                // 攻撃力をスケール
                var attack = (int)(SceneController.StageData.StageCustomData.EnemyAttackScale * Attack);
                
                // シーンコントローラーにゴール到達を通知
                SceneController.EnterEnemy(attack);

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
            if (position.y <= GetHeight())
            {
                // ジャンプ中フラグを解除
                _ccStatus = EnemyCCStatus.None;

                // ダメージ処理
                Damage(_jumpDamage);
                
                // 着地後の位置を地面に合わせる
                position.y = GetHeight();
            }
        }

        /**
         * ノックバック中処理
         */
        private void KnockBacking(
            int timeDiff,
            Vector3 mazeOrigin)
        {
            if (CurrentPosition == null) throw new Exception("Current position is null");
            
            // 現在と目的地のタイル位置を取得
            var startTileCoordinate = CurrentPosition.ToVector3(mazeOrigin);
            var destinationPosition = _knockBackDestination;
            var nextTileCoordinate = destinationPosition.ToVector3(mazeOrigin);

            // 目的地まで等速で移動
            var moveAmount = (nextTileCoordinate - startTileCoordinate) / 1000 * (KnockBackSpeed * timeDiff);

            var localTransform = transform;

            // 移動
            var position = localTransform.position;
            position += moveAmount;
            localTransform.position = position;

            // 到着した場合
            if (Vector3.Distance(position, nextTileCoordinate) 
                < Math.Sqrt(Math.Pow(0.5f, 2) + Math.Pow(GetHeight(), 2)))
            {
                // ノックバック終了アニメーションを再生
                PlayKnockBackEndAnimation();
                // 現在地を更新
                CurrentPosition = destinationPosition;
                // ノックバック中フラグを解除
                _ccStatus = EnemyCCStatus.None;
                // スタンタイムがある時はスタン
                if (_knockBackStunTime <= 0) return;
                Stun(_knockBackStunTime);
                _knockBackStunTime = 0;
            }
        }

        /**
         * ペラペラ中処理
         */
        private void Flipping(
            int timeDiff
        )
        {
            // ペラペラ中は動かない
            if (CurrentPosition == null) throw new Exception("Current position is null");
            
            _stunDuration -= timeDiff;
            
            // ペラペラ中フラグを解除
            if (_stunDuration <= 0)
                _ccStatus = EnemyCCStatus.None;
        }

        /**
         * ダメージ処理
         */
        public void Damage(int damage)
        {
            HP -= damage;

            // HPが0以下になった場合
            if (HP <= 0)
            {
                // 死亡アニメーションを再生
                PlayDeathAnimation(RemainingLives <= 0);
                
                // 残機がない場合は削除
                if (RemainingLives <= 0) return;
                
                // 残機を減らす
                RemainingLives--;
                
                // 蘇生アニメーション
                PlayReviveAnimation();
            }
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
            InvasionEnemyController enemyController,
            int remainingLives = 0
            )
        {
            // 初期化済みはエラー
            if (Initialized) throw new Exception("This enemy is already initialized.");

            // 座標に変換
            var position = startPosition.ToVector3(mazeController.MazeOrigin);
            // 初期ポイントに移動
            transform.position = position;

            // 初期情報登録
            CurrentPosition = startPosition;
            HP = (int)(hp * sceneController.StageData.StageCustomData.EnemyHpScale); // ステージデータでHPをスケール
            MaxHP = HP;
            Speed = speed;
            Attack = attack;
            SceneController = sceneController;
            _mazeController = mazeController;
            _enemyController = enemyController;
            
            RemainingLives = remainingLives + sceneController.StageData.StageCustomData.EnemyRemainingLivesScale;

            Initialized = true;
        }

        private void OnDestroy()
        {
            generalS2SData.Score += 1;
            
            // 敵の削除処理
            _enemyController.EnemyDestroyed(GetInstanceID());
        }

        public void Jump(int height, int damage)
        {
            // ジャンプ中は何もしない
            if (_ccStatus != EnemyCCStatus.None) return;

            // ジャンプ中フラグを立てる
            _ccStatus = EnemyCCStatus.Jumping;

            // ジャンプ情報を登録
            _jumpDamage = damage;

            // ジャンプ速度を計算
            _jumpSpeed = Mathf.Sqrt(2 * Gravity * height);
        }

        public void KnockBack(int distance, int stunTime)
        {
            // knockバック中は何もしない
            if (_ccStatus != EnemyCCStatus.None) return;
            
            _knockBackStunTime = stunTime;

            // ノックバック中フラグを立てる
            _ccStatus = EnemyCCStatus.KnockBacking;
            
            // アニメーション再生
            PlayKnockBackAnimation();

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
            // 同じ目的地の場合は何もしない
            if (Destination != null && Destination.Equals(setPosition)) return;
            
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

        public void Stun(int duration)
        {
            // CCチェインはしない
            if ( _ccStatus != EnemyCCStatus.None) return;
            
            // ペラペラ中フラグを立てる
            _ccStatus = EnemyCCStatus.Stun;
            _stunDuration = duration;
        }

        public void ResetPath()
        {
            Path = null;
        }

        // パスを再計算してあれば更新
        public void ReCalculationPath()
        {
            // パスを再計算
            var path = _mazeController.GetShortestPath(CurrentPosition, Destination, true);
            if (path == null) return;
            
            // 初期位置に強制移動
            transform.position = CurrentPosition!.ToVector3(_mazeController.MazeOrigin);
            
            Path = path;
        }
        
        // アニメーションの再生系
        // バーチャルメソッドを使って、継承先でオーバーライドする

        /**
         * 死亡アニメーション
         * @param destroy ゲームオブジェクトを削除するか
         */
        protected virtual void PlayDeathAnimation( bool destroy = true)
        {
            // ゲームオブジェクトを削除
            if (destroy)
                Destroy(gameObject);
        }
        
        /**
         * 移動アニメーション
         */
        protected virtual void PlayMoveAnimation(){}
        
        /**
         * 蘇生アニメーション
         */
        protected virtual void PlayReviveAnimation(){}
        
        /**
         * 待機アニメーション
         */
        protected virtual void PlayIdleAnimation(){}
        
        /**
         * knockバックの最初のアニメーション
         */
        protected virtual void PlayKnockBackAnimation(){}
        
        /**
         * ノックバックの最後のアニメーション
         */
        protected virtual void PlayKnockBackEndAnimation(){}
        
        // ============= 抽象メソッド =============
        protected abstract float GetHeight();
    }
}