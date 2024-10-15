using System;
using Enums;
using Map.UI.Buttons;
using UnityEngine;

namespace Map.UI
{
    public class MapUIController : MonoBehaviour
    {
        [SerializeField]
        private MapController mapController;

        [SerializeField]
        private MapBossTileButton mapBossTileButton;

        [SerializeField]
        private MapEliteTileButton mapEliteTileButton;

        [SerializeField]
        private MapNormalTileButton mapNormalTileButton;

        [SerializeField]
        private MapStartTileButton mapStartTileButton;

        [SerializeField]
        private MapEventTileButton mapEventTileButton;

        [SerializeField]
        private GameObject mapRowPrefab;

        [SerializeField]
        private GameObject mapWrapper;

        private void Start()
        {
            // 現在のマップUIを削除
            foreach (Transform child in mapWrapper.transform) Destroy(child.gameObject);

            // 現在のマップを
            var map = mapController.GetCurrentMap();

            //　行ごとに生成
            for (var i = 0; i < map.rowCount; i++)
            {
                // マップの行を取得
                var row = map.GetRow(i);

                // 行のラッパーを生成
                var mapRow = Instantiate(mapRowPrefab, mapWrapper.transform, true);

                // 行のラッパーをマップの子要素にする
                foreach (var mapTile in row)
                {
                    AMapTileButton button = null;

                    switch (mapTile.type)
                    {
                        case MapTileType.Start:
                            button = Instantiate(mapStartTileButton);
                            break;
                        case MapTileType.Normal:
                            button = Instantiate(mapNormalTileButton, mapRow.transform);
                            break;
                        case MapTileType.Elite:
                            button = Instantiate(mapEliteTileButton, mapRow.transform);
                            break;
                        case MapTileType.Boss:
                            button = Instantiate(mapBossTileButton, mapRow.transform);
                            break;
                        case MapTileType.Event:
                            button = Instantiate(mapEventTileButton, mapRow.transform);
                            break;
                        default:
                            throw new Exception("Invalid MapTileType");
                    }

                    // 列の子に追加
                    button.transform.SetParent(mapRow.transform);
                }
            }

            // マップを非表示にする
            gameObject.SetActive(false);
        }

        /** 現在のマップを表示する */
        public void ShowCurrentMap()
        {
            // マップを表示する
            gameObject.SetActive(true);
        }

        /** マップを非表示にする */
        public void HideMap()
        {
            gameObject.SetActive(false);
        }
    }
}