using AClass;
using UI.Abstract;
using UI.Generator;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DetailViewerController : MonoBehaviour
    {
        [SerializeField]
        private CardGenerator cardGenerator;
        
        [SerializeField]
        private GameObject wrapper;
        
        private AGeneralCard _currentCard;
        
        /**
         * トラップの詳細を表示する
         */
        public void ShowTrapDetail(ATrap trap)
        {
            CloseDetail();
            
            gameObject.SetActive(true);
            _currentCard = cardGenerator.GetTrapCard(trap.GetTrapName());
            _currentCard = Instantiate(_currentCard, wrapper.transform, false);
        }
        
        /**
         * スキルの詳細を表示する
         */
        public void ShowSkillDetail(ASkill skill)
        {
            CloseDetail();
            
            gameObject.SetActive(true);
            _currentCard = cardGenerator.GetSkillCard(skill.GetSkillName());
            _currentCard = Instantiate(_currentCard, wrapper.transform, false);
        }
        
        /**
         * タレットの詳細を表示する
         */
        public void ShowTurretDetail(ATurret turret)
        {
            CloseDetail();
            
            gameObject.SetActive(true);
            _currentCard = cardGenerator.GetTurretCard(turret.GetTurretName());
            _currentCard = Instantiate(_currentCard, wrapper.transform, false);
        }
        
        /**
         * 詳細を閉じる
         */
        public void CloseDetail()
        {
            gameObject.SetActive(false);
            if (_currentCard != null)
            {
                Destroy(_currentCard.gameObject);
                _currentCard = null;
            }
        }

        /**
         * 詳細カードのサイズ位置調整
         */
        private void Update()
        {
            if (_currentCard == null) return;
            
            var cardTransform = _currentCard.transform;
            cardTransform.localScale = new Vector3(7, 7, 7);
            cardTransform.localPosition = new Vector3(-200, 0, 0);
        }
    }
}