using System.Collections.Generic;
using UI.Icon;
using UnityEngine;

namespace Deck
{
    public class DeckIconGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<GeneralTrapIcon> _deckTrapIcons;
        
        [SerializeField]
        private List<GeneralSkillIcon> _deckSkillIcons;

        [SerializeField]
        private List<GeneralTurretIcon> _deckTurretIcons;

        public GeneralTrapIcon GenerateTrapIcon(string trapName)
        {
            foreach (var deckTrapIcon in _deckTrapIcons)
            {
                if (deckTrapIcon.GetTrapName() == trapName)
                    return deckTrapIcon;
            }

            return null;
        }

        public GeneralSkillIcon GenerateSkillIcon(string skillName)
        {
            foreach (var deckSkillIcon in _deckSkillIcons)
            {
                if (deckSkillIcon.GetSkillName() == skillName)
                    return deckSkillIcon;
            }

            return null;
        }

        public GeneralTurretIcon GenerateTurretIcon(string turretName)
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