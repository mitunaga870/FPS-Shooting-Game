using System;
using AClass;
using UI.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Card
{
    public class TrapCard : AGeneralCard
    {
        [SerializeField]
        private ATrap trapPrefab;
        
        public string GetTrapName()
        {
            return trapPrefab.GetTrapName();
        }
        
        public void SetButtonAction(Action action)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => action());
        }
    }
}