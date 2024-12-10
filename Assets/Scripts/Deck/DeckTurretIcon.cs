using AClass;
using UI;
using UnityEngine;

namespace Deck
{
    public class DeckTurretIcon : ADeckIcon
    {
        [SerializeField]
        private ATurret turret;
        
        public string GetTurretName()
        {
            return turret.GetTurretName();
        }

        public override void SetDeckUIController(DeckUIController deckUIController)
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => deckUIController.ShowTurretInfo(turret));
        }
    }
}