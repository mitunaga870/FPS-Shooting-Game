using AClass;
using UI;
using UnityEngine;

namespace Deck
{
    public class DeckTrapIcon : ADeckIcon
    {
        [SerializeField]
        private ATrap trap;
        
        public string GetTrapName()
        {
            return trap.GetTrapName();
        }

        public override void SetDeckUIController(DeckUIController deckUIController)
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => deckUIController.ShowTrapInfo(trap));
        }
    }
}