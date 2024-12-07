using System.Collections.Generic;
using UnityEngine;

namespace Deck
{
    public class DeckIconGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<DeckTrapIcon> _deckTrapIcons;
        
        [SerializeField]
        private List<DeckSkillIcon> _deckSkillIcons;

        [SerializeField]
        private List<DeckTurretIcon> _deckTurretIcons;

        public DeckTrapIcon GenerateTrapIcon(string trapName)
        {
            foreach (var deckTrapIcon in _deckTrapIcons)
            {
                if (deckTrapIcon.GetTrapName() == trapName)
                    return deckTrapIcon;
            }

            return null;
        }

        public DeckSkillIcon GenerateSkillIcon(string skillName)
        {
            foreach (var deckSkillIcon in _deckSkillIcons)
            {
                if (deckSkillIcon.GetSkillName() == skillName)
                    return deckSkillIcon;
            }

            return null;
        }

        public DeckTurretIcon GenerateTurretIcon(string turretName)
        {
            foreach (var deckTurretIcon in _deckTurretIcons)
            {
                if (deckTurretIcon.GetTurretName() == turretName)
                    return deckTurretIcon;
            }

            return null;
        }
    }
}