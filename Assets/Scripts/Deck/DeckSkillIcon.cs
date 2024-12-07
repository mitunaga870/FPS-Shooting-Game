using AClass;
using UI;
using UnityEngine;

namespace Deck
{
    public class DeckSkillIcon : ADeckIcon
    {
        [SerializeField]
        private ASkill skill;
        
        public string GetSkillName()
        {
            return skill.GetSkillName();
        }

        public override void SetDeckUIController(DeckUIController deckUIController)
        {
            var button = GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => deckUIController.ShowSkillInfo(skill));
        }
    }
}