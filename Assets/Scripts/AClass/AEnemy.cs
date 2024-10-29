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

        /** 作成フェーズから侵略フェーズへのデータ */
        [SerializeField]
        private CreateToInvasionData c2IData;

        /** Sceneコントローラー */
        private InvasionController _sceneController;

        /** 迷路コントローラー */
        private InvasionMazeController _mazeController;

        /** 現在の経路のインデックス */
        private int? CurrentPathIndex => Path?.Index(CurrentPosition);

        private InvasionEnemyController _enemyController;

        private int _prevTime;

        private int atk = 1;

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

            // 目的地がない場合はゴールに指定
            Destination ??= _mazeController.GoalPosition;

            // 経路がない場合は生成
            Path ??= _mazeController.GetShortestPath(CurrentPosition, _mazeController.GoalPosition);

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
            if (CurrentPosition.Equals(Destination)) Path = null;

            // ゴールに到達した場合
            if (CurrentPosition.Equals(_mazeController.GoalPosition))
            {
                _sceneController.EnterEnemy(atk);

                // ゲームオブジェクトを削除
                Destroy(gameObject);
            }

            // ロード済み時間を更新
            _prevTime = time;
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
        public void Initialize(int hp, int speed, TilePosition startPosition, InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController)
        {
            // 初期化済みはエラー
            if (Initialized) throw new Exception("This emeny is already initialized.");

            // 座標に変換
            var position = startPosition.ToVector3(mazeController.MazeOrigin);
            // 初期ポイントに移動
            transform.position = position;

            // 初期情報登録
            CurrentPosition = startPosition;
            HP = hp;
            Speed = speed;
            _sceneController = sceneController;
            _mazeController = mazeController;
            _enemyController = enemyController;

            Initialized = true;
        }

        private void OnDestroy()
        {
            // 敵の削除処理
            _enemyController.EnemyDestroyed();
        }
    }
}