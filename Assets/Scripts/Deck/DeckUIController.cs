using System.Collections.Generic;
using AClass;
using Deck;
using UI.Abstract;
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
        
        private List<AGeneralIcon> _deckIcons;
        
        private List<ATrap> _deckTraps;
        private List<ASkill> _deckSkills;
        private List<ATurret> _deckTurrets;
    
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
            
            _deckIcons = new List<AGeneralIcon>();
            
            // 読み込む
            (_deckTraps, _deckSkills, _deckTurrets) = deckController.LoadDeck();
            
            // 一括表示
            foreach (var trap in _deckTraps)
            {
                var trapIcon = deckIconGenerator.GenerateTrapIcon(trap.GetTrapName());
                
                // すでに表示されている場合はスキップ
                if (_deckIcons.Contains(trapIcon))
                    continue;
                
                // 追加
                _deckIcons.Add(trapIcon);
                
                // 表示
                trapIcon = Instantiate(trapIcon, deckListWrapper.transform, false);
                trapIcon.SetClickAction(() => ShowTrapInfo(trap));
                // 個数を設定
                trapIcon.SetAmount(_deckTraps.FindAll(t => t.GetTrapName() == trap.GetTrapName()).Count);
            }

            foreach (var skill in _deckSkills)
            {
                var skillIcon = deckIconGenerator.GenerateSkillIcon(skill.GetSkillName());
                
                // すでに表示されている場合はスキップ
                if (_deckIcons.Contains(skillIcon))
                    continue;
                
                // 追加
                _deckIcons.Add(skillIcon);
                
                // 表示
                skillIcon = Instantiate(skillIcon, deckListWrapper.transform, false);
                skillIcon.SetClickAction(() => ShowSkillInfo(skill));
                // 個数を設定
                skillIcon.SetAmount(_deckSkills.FindAll(s => s.GetSkillName() == skill.GetSkillName()).Count);
            }

            foreach (var turret in _deckTurrets)
            {
                var turretIcon = deckIconGenerator.GenerateTurretIcon(turret.GetTurretName());
                
                // すでに表示されている場合はスキップ
                if (_deckIcons.Contains(turretIcon))
                    continue;
                
                // 追加
                _deckIcons.Add(turretIcon);
                
                // 表示
                turretIcon = Instantiate(turretIcon, deckListWrapper.transform, false);
                turretIcon.SetClickAction(() => ShowTurretInfo(turret));
                // 個数を設定
                turretIcon.SetAmount(_deckTurrets.FindAll(t => t.GetTurretName() == turret.GetTurretName()).Count);
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
            turretCard = Instantiate(turretCard, cardInfoWrapper.transform, false);
            
            // 個数を設定
            turretCard.SetAmount(_deckTurrets.FindAll(t => t.GetTurretName() == turret.GetTurretName()).Count);
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
            trapCard = Instantiate(trapCard, cardInfoWrapper.transform, false);
            
            // 個数を設定
            trapCard.SetAmount(_deckTraps.FindAll(t => t.GetTrapName() == trap.GetTrapName()).Count);
        }
        
        public void ShowSkillInfo(ASkill skill)
        {
            // ディティールを消す
            foreach (Transform child in cardInfoWrapper.transform)
            {
                Destroy(child.gameObject);
            }
            
            // ディティールを表示
            var skillCard = cardGenerator.GetSkillCard(skill.GetSkillName());
            skillCard = Instantiate(skillCard, cardInfoWrapper.transform, false);
            
            // 個数を設定
            skillCard.SetAmount(_deckSkills.FindAll(s => s.GetSkillName() == skill.GetSkillName()).Count);
        }
    }
}
