using AClass;
using UnityEngine;

namespace InvasionPhase.UI
{
    public class SkillIcon : MonoBehaviour
    {
        [SerializeField] private ASkill _skill;
        [SerializeField] private InvasionController _sceneController;

        public async void SetSelectPositionMode()
        {
            _sceneController.SetSkillMode(_skill);
        }
    }
}
