using AClass;
using ScriptableObjects.S2SDataObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StageText : MonoBehaviour
    {
        [SerializeField]
        private GeneralS2SData generalS2SData;
        
        [SerializeField]
        private TextMeshProUGUI text;
        
        private void Start()
        {
            text.text = $"STAGE {generalS2SData.MapNumber + 1}-{generalS2SData.CurrentStageNumber}";
        }
    }
}
