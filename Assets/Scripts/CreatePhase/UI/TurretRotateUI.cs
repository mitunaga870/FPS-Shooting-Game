using UnityEngine;
using UnityEngine.UIElements;

namespace CreatePhase.UI
{
    public class TurretRotateUI : MonoBehaviour
    {
        [SerializeField]
        private IPanel topPanel;

        [SerializeField]
        private IPanel bottomPanel;

        [SerializeField]
        private IPanel leftPanel;

        [SerializeField]
        private IPanel rightPanel;

        public void Show()
        {
            Debug.Log("VAR");
            gameObject.SetActive(true);
        }
    }
}