using System;
using AClass;
using UI.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Card
{
    public class SkillCard : AGeneralCard
    {
        [SerializeField]
        private ASkill trapPrefab;
        
        public string GetSkillName()
        {
            return trapPrefab.GetSkillName();
        }

        public void SetButtonAction(Action action)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => action());
        }
    }
}