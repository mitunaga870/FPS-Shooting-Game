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
        private GeneralS2SData generalS2SData;

        [SerializeField]
        private CreateToInvasionData c2IData;

        private MapWrapper _mapWrapper;
        private MapTile _mapTile;
        private bool _isCurrent;
        private Phase _currentPhase;

        public void Init(MapWrapper mapWrapper, MapTile mapTile)
        {
            _mapWrapper = mapWrapper;
            _mapTile = mapTile;

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
            switch (SceneManager.GetActiveScene().name)
            {
                case "CreatePhase":
                    _currentPhase = Phase.Create;
                    break;
                case "InvasionPhase":
                    _currentPhase = Phase.Invade;

                    // 隣接しているマップのみクリック可能
                    if (_mapWrapper.IsNextToCurrentMap(
                            generalS2SData.CurrentMapRow,
                            generalS2SData.CurrentMapColumn,
                            _mapTile.Row,
                            _mapTile.Column
                        ))
                        gameObject.GetComponent<Button>().interactable = true;
                    break;
            }
        }

        public void HandleClickButton()
        {
            switch (_currentPhase)
            {
                case Phase.Create:
                    break;
                case Phase.Invade:
                    MoveNextMap();
                    break;
            }
        }

        /**
         * 次のマップへの移動を行う
         */
        private void MoveNextMap()
        {
            generalS2SData.CurrentMapRow = _mapTile.Row;
            generalS2SData.CurrentMapColumn = _mapTile.Column;

            c2IData.Reset();

            // シーン遷移
            SceneManager.LoadScene("CreatePhase");
        }
    }
}