using DataClass;
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

        // Start is called before the first frame update
        public void Start()
        {
            mazeController.Create(createToInvasionData.TileData, createToInvasionData.TrapData);
        }
    }
}