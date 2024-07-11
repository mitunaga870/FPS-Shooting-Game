using DataClass;
using Enemies;
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
    }
}