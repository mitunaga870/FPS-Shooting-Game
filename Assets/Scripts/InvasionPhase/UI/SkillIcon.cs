using AClass;
using TMPro;
using UnityEngine;

namespace InvasionPhase.UI
{
    public class SkillIcon : MonoBehaviour
    {
        [SerializeField] private ASkill _skill;

        [SerializeField]
        private TextMeshProUGUI _skillRemainingCountText;
        
        private InvasionController _sceneController;
        
        /** スキルの残り使用回数*/
        private int _remainingCount = 1;

        private void Update()
        {
            _skillRemainingCountText.text = _remainingCount.ToString();
            
            // スキルの残り使用回数が0になったら非表示
            if (_remainingCount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void Init(InvasionController sceneController)
        {
            _sceneController = sceneController;
        }

        public void SetSelectPositionMode()
        {
            if (_sceneController == null) return;
            
            _sceneController.SetSkillMode(_skill);
        }

        public void IncreaseCount()
        {
            _remainingCount++;
        }

        public void DecreaseCount()
        {
            _remainingCount--;
        }
    }
}
