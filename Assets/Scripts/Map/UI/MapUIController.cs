using System;
using Enums;
using Map.UI.Buttons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Map.UI
{
    public class MapUIController : MonoBehaviour
    {
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
        private MapShopTileButton mapShopTileButton;

        [SerializeField]
        private GameObject mapRowPrefab;

        [SerializeField]
        private GameObject mapUIWrapper;
        
        private bool _isClosable;
        private DeckController _deckController;
        private MapController _mapController;

        /** マップを表示する */
        public void Load(MapController mapController, DeckController deckController, WalletController walletController, bool isClosable)
        {
            _isClosable = isClosable;
            _deckController = deckController;
            _mapController = mapController;
            
            // 現在のマップを
            var map = mapController.GetCurrentMap();

            //　行ごとに生成
            for (var i = 0; i < map.RowCount; i++)
            {
                // マップの行を取得
                var row = map.GetRow(i);

                // 行のラッパーを生成
                var mapRow = Instantiate(mapRowPrefab, mapUIWrapper.transform, true);

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
                        case MapTileType.Shop:
                            button = Instantiate(mapShopTileButton, mapRow.transform);
                            ((MapShopTileButton)button).Initialize(deckController, walletController);
                            break;
                        default:
                            throw new Exception("Invalid MapTileType");
                    }

                    // 列の子に追加
                    button.transform.SetParent(mapRow.transform);
                    // ボタンの初期化
                    button.Init(map, mapTile, mapController, this);
                }
            }
        }

        /** マップを非表示にする */
        public void HideMap()
        {
            if (!_isClosable) return; 
            
            // マップを閉じる
            _mapController.IsMapOpen = false;
            
            Destroy(gameObject);
        }
    }
}