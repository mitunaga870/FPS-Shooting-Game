using System.Collections.Generic;
using AClass;
using Enums;
using InvasionPhase;
using UnityEngine;
using UnityEngine.UI;

namespace Reward
{
    public class RewardUIController : MonoBehaviour
    {

        #region SerializeField
        
        [SerializeField]
        private InvasionController invasionController;
        
        [SerializeField]
        private List<GameObject> rewardItemWrappers;
        
        [SerializeField]
        private List<GameObject> creditIcons;
        
        [SerializeField]
        private TMPro.TextMeshProUGUI rewardMoneyText;

        #endregion

        #region Private Variables
        
        private int rewardMoney;
        
        private List<(ATrap, ATurret, ASkill)> rewardItems;
        private List<RewardType> rewardTypes;
        
        private (ATrap, ATurret, ASkill) selectedRewardItem;
        private RewardType selectedRewardType;
        
        #endregion
        public bool IsRewardUIShowing => gameObject.activeSelf;
        

        /**
         * 報酬画面を表示する
         */
        public void ShowRewardUI(
            StageType stageType,
            // ReSharper disable once ParameterHidesMember
            int rewardMoney,
            List<ATrap> rewardTraps,
            List<ASkill> rewardSkills,
            List<ATurret> rewardTurrets
        ) {
            // 報酬アイテムが３つ以上ある場合はエラー
            if (rewardTraps.Count + rewardSkills.Count + rewardTurrets.Count > 3)
            {
                Debug.LogError("報酬アイテムが３つ以上あります");
                return;
            }
            
            // 報酬画面を表示
            gameObject.SetActive(true);
            
            // 変数に報酬情報を格納
            this.rewardMoney = rewardMoney;
            
            rewardItems = new List<(ATrap, ATurret, ASkill)>();
            rewardTypes = new List<RewardType>();

            foreach (var trap in rewardTraps)
            {
                rewardItems.Add((trap, null, null));
                rewardTypes.Add(RewardType.Trap);
            }
            foreach (var turret in rewardTurrets)
            {
                rewardItems.Add((null, turret, null));
                rewardTypes.Add(RewardType.Turret);
            }

            foreach (var skill in rewardSkills)
            {
                rewardItems.Add((null, null, skill));
                rewardTypes.Add(RewardType.Skill);
            }
            
            // ステージの種類でクレジットアイコン数を変える
            switch (stageType)
            {
                case StageType.Normal:
                    creditIcons[0].SetActive(true);
                    creditIcons[1].SetActive(false);
                    creditIcons[2].SetActive(false);
                    break;
                case StageType.Elite:
                    creditIcons[0].SetActive(true);
                    creditIcons[1].SetActive(true);
                    creditIcons[2].SetActive(false);
                    break;
                case StageType.Boss:
                    creditIcons[0].SetActive(true);
                    creditIcons[1].SetActive(true);
                    creditIcons[2].SetActive(true);
                    break;
            }
            
            // 報酬金額を表示
            rewardMoneyText.text = rewardMoney.ToString();
        }


        public void OnItem1Button() {
            //一番左のトラップが押されたら
            rewardItemWrappers[0].GetComponent<Image>().enabled = true;
            rewardItemWrappers[1].GetComponent<Image>().enabled = false;
            rewardItemWrappers[2].GetComponent<Image>().enabled = false;
            
            // 選択された報酬アイテムを変数に格納
            selectedRewardItem = rewardItems[0];
            selectedRewardType = rewardTypes[0];
        }

        public void OnItem2Button() {
            //真ん中のトラップが押されたら
            rewardItemWrappers[0].GetComponent<Image>().enabled = false;
            rewardItemWrappers[1].GetComponent<Image>().enabled = true;
            rewardItemWrappers[2].GetComponent<Image>().enabled = false;
            
            // 選択された報酬アイテムを変数に格納
            selectedRewardItem = rewardItems[1];
            selectedRewardType = rewardTypes[1];
        }

        public void OnItem3Button() {
            //一番右のトラップが押されたら
            rewardItemWrappers[0].GetComponent<Image>().enabled = false;
            rewardItemWrappers[1].GetComponent<Image>().enabled = false;
            rewardItemWrappers[2].GetComponent<Image>().enabled = true;
            
            // 選択された報酬アイテムを変数に格納
            selectedRewardItem = rewardItems[2];
            selectedRewardType = rewardTypes[2];
        }

        public void SubmitSelection()
        {
            invasionController.ReceiveReward(rewardMoney, selectedRewardItem, selectedRewardType);
        }
    }
}
