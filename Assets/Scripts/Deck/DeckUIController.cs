using System.Collections.Generic;
using AClass;
using Deck;
using UI.Generator;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DeckUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject deckListWrapper;
    
        [SerializeField]
        private DeckController deckController;
        
        [SerializeField]
        private DeckIconGenerator deckIconGenerator;

        [SerializeField]
        private CardGenerator cardGenerator;
        
        [SerializeField]
        private GameObject cardInfoWrapper;
        
        public bool IsDeckUIShowing => gameObject.activeSelf;
        
        private bool _isInitialized;
    
        public void ShowDeckUI()
        {
            // 初期化
            Init();
            
            // 表示させる
            gameObject.SetActive(true);
        }
        
        public void HideDeckUI()
        {
            // 非表示にする
            gameObject.SetActive(false);
        }

        private void Init()
        {
            if (_isInitialized)
                return;
            
            _isInitialized = true;
            
            // 読み込む
            var (traps, skills, turrets) = deckController.LoadDeck();
            
            // 一括表示
            foreach (var trap in traps)
            {
                var trapIcon = deckIconGenerator.GenerateTrapIcon(trap.GetTrapName());
                trapIcon = Instantiate(trapIcon, deckListWrapper.transform, false);
                trapIcon.SetDeckUIController(this);
            }

            foreach (var skill in skills)
            {
                var skillIcon = deckIconGenerator.GenerateSkillIcon(skill.GetSkillName());
                skillIcon = Instantiate(skillIcon, deckListWrapper.transform, false);
                skillIcon.SetDeckUIController(this);
            }

            foreach (var turret in turrets)
            {
                var turretIcon = deckIconGenerator.GenerateTurretIcon(turret.GetTurretName());
                turretIcon = Instantiate(turretIcon, deckListWrapper.transform, false);
                turretIcon.SetDeckUIController(this);
            }
        }

        public void ShowTurretInfo(ATurret turret)
        {
            // ディティールを消す
            foreach (Transform child in cardInfoWrapper.transform)
            {
                Destroy(child.gameObject);
            }
            
            // ディティールを表示
            var turretCard = cardGenerator.GetTurretCard(turret.GetTurretName());
            Instantiate(turretCard, cardInfoWrapper.transform, false);
        }
        
        public void ShowTrapInfo(ATrap trap)
        {
            // ディティールを消す
            foreach (Transform child in cardInfoWrapper.transform)
            {
                Destroy(child.gameObject);
            }
            
            // ディティールを表示
            var trapCard = cardGenerator.GetTrapCard(trap.GetTrapName());
            Instantiate(trapCard, cardInfoWrapper.transform, false);
        }
        
        public void ShowSkillInfo(ASkill skill)
        {
            // ディティールを消す
            foreach (Transform child in cardInfoWrapper.transform)
            {
                Destroy(child.gameObject);
            }
            
            // ディティールを表示
            // ディティールを消す
            foreach (Transform child in cardInfoWrapper.transform)
            {
                Destroy(child.gameObject);
            }
            
            // ディティールを表示
            var skillCard = cardGenerator.GetSkillCard(skill.GetSkillName());
            Instantiate(skillCard, cardInfoWrapper.transform, false);
        }
    }
}
