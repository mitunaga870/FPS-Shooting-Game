using System;
using System.Linq;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Map
{
    [DefaultExecutionOrder(-1)]
    public class MapController : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        private const int STAGE_COUNT = 4;

        [SerializeField]
        private MapObject mapObject;

        [SerializeField]
        private GeneralS2SData generalS2SData;

        /**
         * マップのラッパー
         * ステージごとに配列要素とする
         */
        [NonSerialized]
        private MapWrapper[] _mapWrappers;

        [NonSerialized]
        private MapWrapper test;

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            if (generalS2SData.Maps != null)
            {
                // すでにマップデータがある時はそれを使う
                _mapWrappers = generalS2SData.Maps;
                return;
            }

            var saveData = SaveController.LoadMap();

            if (saveData != null)
            {
                // セーブデータがある時はそれを使う
                _mapWrappers = new MapWrapper[saveData.Length];
                for (var i = 0; i < saveData.Length; i++) _mapWrappers[i] = saveData[i];
            }
            else
            {
                // セーブデータがない時は新規作成
                _mapWrappers = new MapWrapper[STAGE_COUNT];

                // 各ステージのマップを生成
                _mapWrappers[0] = new MapWrapper(mapObject.FirstStage);
                _mapWrappers[1] = new MapWrapper(mapObject.SecondStage);
                _mapWrappers[2] = new MapWrapper(mapObject.ThirdStage);
                _mapWrappers[3] = new MapWrapper(mapObject.FourthStage);
            }

            generalS2SData.Maps = _mapWrappers;
        }

        public MapWrapper GetCurrentMap()
        {
            var mapNumber = generalS2SData.MapNumber;

            // マップナンバー負あるいは範囲越えの時はエラー
            if (mapNumber < 0 || mapNumber >= _mapWrappers.Length)
                throw new Exception("マップナンバーが不正です");

            return _mapWrappers[mapNumber];
        }

        public void SaveMap()
        {
            SaveController.SaveMap(_mapWrappers);

            SaveController.SaveCurrentMapNumber(generalS2SData.MapNumber);
            SaveController.SaveCurrentMapRow(generalS2SData.CurrentMapRow);
            SaveController.SaveCurrentMapColumn(generalS2SData.CurrentMapColumn);
        }

        private void OnApplicationQuit()
        {
            SaveMap();
        }
    }
}