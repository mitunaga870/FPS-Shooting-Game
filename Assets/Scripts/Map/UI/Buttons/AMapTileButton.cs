using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Map.UI.Buttons
{
    public abstract class AMapTileButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;

        private MapWrapper mapWrapper;
        private MapTile mapTile;

        public void Init(MapWrapper mapWrapper, MapTile mapTile)
        {
            this.mapWrapper = mapWrapper;
            this.mapTile = mapTile;

            label.text = mapTile.ToString();
        }

        public void CheckArrayAddress()
        {
            Debug.Log(mapWrapper.ConvertToArrayAddress(mapTile.Row, mapTile.Column));
        }
    }
}