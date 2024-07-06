using ScriptableObjects.S2SDataObjects;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionController : MonoBehaviour
    {
        /** シーン間のデータ共有オブジェクト */
        [SerializeField] private CreateToInvasionData createToInvasionData;

        // Start is called before the first frame update
        public void Start()
        {
            Debug.Log(createToInvasionData);
        }
    }
}