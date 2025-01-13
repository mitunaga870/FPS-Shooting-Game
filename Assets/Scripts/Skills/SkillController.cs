using System.Collections.Generic;
using AClass;
using InvasionPhase;
using InvasionPhase.UI;
using UI;
using UnityEngine;

namespace Skills
{
    public class SkillController : MonoBehaviour
    {
        [SerializeField]
        private DeckController deckController;
        
        [SerializeField]
        private GameObject skillUIWrapper;
        
        [SerializeField]
        private InvasionController sceneController;
        
        [SerializeField]
        private DetailViewerController detailViewerController;
        
        private Dictionary<string, SkillIcon> _addedSkills = new Dictionary<string, SkillIcon>();
        
        private void Start()
        {
            // デッキを読んでUIに表示
            var deckSkills = deckController.DrawSkills();

            foreach (var skill in deckSkills)
            {
                var skillName = skill.GetSkillName();
                
                // 既に追加済みのスキルは追加せず所持数を増やす
                if (_addedSkills.ContainsKey(skillName))
                {
                    _addedSkills[skillName].IncreaseCount();
                }
                else
                {
                    var skillObject = Instantiate(skill, skillUIWrapper.transform, false);
                    var icon = skillObject.GetComponent<SkillIcon>();

                    // スキルを追加
                    _addedSkills[skillName] = icon;
                    
                    skillObject.Init(this);
                    icon.Init(sceneController, detailViewerController);
                }
            }
        }

        /**
         * スキルを使用する
         */
        public void Use(ASkill aSkill)
        {
            // スキル名を取得
            var skillName = aSkill.GetSkillName();
            
            // デッキに通知
            deckController.UseSkill(skillName);
            
            // スキルの残り使用回数を減らす
            _addedSkills[skillName].DecreaseCount();
        }
    }
}