using Enums;
using ScriptableObjects.S2SDataObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Map.UI.Buttons
{
    public abstract class AMapTileButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;

        [SerializeField]
        protected GeneralS2SData generalS2SData;

        [SerializeField]
        private CreateToInvasionData c2IData;

        private MapWrapper _mapWrapper;
        protected MapTile MapTile;
        private bool _isCurrent;
        private Phase _currentPhase;
        
        protected MapController MapController;
        protected MapUIController MapUIController;

        public void Init(
            MapWrapper mapWrapper,
            MapTile mapTile,
            MapController mapController,
            MapUIController mapUIController
            )
        {
            _mapWrapper = mapWrapper;
            MapTile = mapTile;
            MapController = mapController;
            MapUIController = mapUIController;

            // タイプでラベルを変える
            label.text = mapTile.ToLabelString();

            // 現在地の場合はアウトライン
            if (mapTile.Row == generalS2SData.CurrentMapRow && mapTile.Column == generalS2SData.CurrentMapColumn)
            {
                _isCurrent = true;
                gameObject.GetComponent<UnityEngine.UI.Outline>().enabled = true;
            }

            // 基本はクリックできないように
            gameObject.GetComponent<Button>().interactable = false;

            // 現在シーンによって表示形式を変える
            // 隣接しているマップのみクリック可能
            if ( MapController.IsMoveMap &&
                _mapWrapper.IsNextToCurrentMap(
                    generalS2SData.CurrentMapRow,
                    generalS2SData.CurrentMapColumn,
                    MapTile.Row,
                    MapTile.Column
                ))
                gameObject.GetComponent<Button>().interactable = true;
        }

        public void HandleClickButton()
        {
            if (MapController.IsMoveMap)
                MoveNextMap();
        }

        /**
         * 次のマップへの移動を行う
         */
        protected virtual void MoveNextMap()
        {
            generalS2SData.CurrentMapRow = MapTile.Row;
            generalS2SData.CurrentMapColumn = MapTile.Column;

            c2IData.Reset();

            // シーン遷移
            SceneManager.LoadScene("CreatePhase");
        }
    }
}