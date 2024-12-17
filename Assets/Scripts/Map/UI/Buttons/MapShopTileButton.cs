using Shop;
using UnityEngine;

namespace Map.UI.Buttons
{
    public class MapShopTileButton : AMapTileButton
    {
        [SerializeField]
        private ShopController _shopUI;
        
        private DeckController _deckController;
        private WalletController _walletController;
        
        protected override void MoveNextMap()
        {
            // マップを閉じる
            Destroy(MapUIController.gameObject);
            
            // S2Sデータのマップナンバーを進める
            generalS2SData.CurrentMapRow = MapTile.Row;
            generalS2SData.CurrentMapColumn = MapTile.Column;
            generalS2SData.CurrentStageNumber++;
            
            // 次に遷移する変わりにショップを開く
            var shop = Instantiate(_shopUI);
            shop.Initialize(_deckController, _walletController);
            
            // ショップが閉じられた時の処理を追加
            shop.SetOnClose(() =>
            {
                shop.gameObject.SetActive(false);
                // マップを開く
                MapController.ShowMap(false, true);
            });
        }

        public void Initialize(DeckController deckController, WalletController walletController)
        {
            _deckController = deckController;
            _walletController = walletController;
        }
    }
}