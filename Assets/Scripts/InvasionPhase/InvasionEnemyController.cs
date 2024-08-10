using DataClass;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionEnemyController : MonoBehaviour
    {
        [SerializeField] private GeneralS2SData _generalS2SData;

        [SerializeField] private StageObject _stageObject;

        [SerializeField] private InvasionController _invasionController;

        public void Update()
        {
            // ゲーム自国の追加
            var time = _invasionController.GameTime;
        }
    }
}