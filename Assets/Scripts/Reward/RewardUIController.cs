using System;
using System.Collections.Generic;
using System.Linq;
using AClass;
using Enums;
using InvasionPhase;
using UI.Generator;
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

        [SerializeField]
        private IconGenerator iconGenerator;

        #endregion

        #region Private Variables
        
        private int _rewardMoney;

        private int _selectableRewardAmount;
        
        private List<(ATrap, ATurret, ASkill)> _rewardItems;
        private List<RewardType> _rewardTypes;
        
        private List<(ATrap, ATurret, ASkill)> _selectedRewardItem;
        private List<RewardType> _selectedRewardType;
        
        private List<bool> _selectedRewardItemWrappers;
        
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
            List<ATurret> rewardTurrets,
            int selectableRewardAmount
        ) {
            // 報酬アイテムがラッパーの数より多い場合はエラーを出力
            if (rewardTraps.Count + rewardSkills.Count + rewardTurrets.Count > rewardItemWrappers.Count)
            {
                Debug.LogError("報酬アイテムがラッパーの数より多いです");
                return;
            }
            
            // 選択可能な報酬アイテムの数を設定
            this._selectableRewardAmount = selectableRewardAmount;
            
            // 選択インデックスの追加済みフラグを初期化
            _selectedRewardItemWrappers = new List<bool>();
            
            // 報酬画面を表示
            gameObject.SetActive(true);
            
            // 変数に報酬情報を格納
            this._rewardMoney = rewardMoney;
            
            _rewardItems = new List<(ATrap, ATurret, ASkill)>();
            _rewardTypes = new List<RewardType>();
            
            // ラッパーが何個目かのインデックス
            var wrapperIndex = 0;

            foreach (var trap in rewardTraps)
            {
                _rewardItems.Add((trap, null, null));
                _rewardTypes.Add(RewardType.Trap);
                _selectedRewardItemWrappers.Add(false);
                
                // アイコンを生成
                var icon = iconGenerator.GenerateTrapIcon(trap);
                icon = Instantiate(icon, rewardItemWrappers[wrapperIndex].transform);
                
                // アイコンにクリックイベントを追加
                var i = wrapperIndex;
                icon.SetClickAction(() => SelectReward(i));
                
                wrapperIndex++;
            }
            foreach (var turret in rewardTurrets)
            {
                _rewardItems.Add((null, turret, null));
                _rewardTypes.Add(RewardType.Turret);
                _selectedRewardItemWrappers.Add(false);
                
                // アイコンを生成
                var icon = iconGenerator.GenerateTurretIcon(turret);
                icon = Instantiate(icon, rewardItemWrappers[wrapperIndex].transform);
                
                // アイコンにクリックイベントを追加
                var i = wrapperIndex;
                icon.SetClickAction(() => SelectReward(i));
                
                wrapperIndex++;
            }

            foreach (var skill in rewardSkills)
            {
                _rewardItems.Add((null, null, skill));
                _rewardTypes.Add(RewardType.Skill);
                _selectedRewardItemWrappers.Add(false);
                
                // アイコンを生成
                var icon = iconGenerator.GenerateSkillIcon(skill);
                icon = Instantiate(icon, rewardItemWrappers[wrapperIndex].transform);
                
                // アイコンにクリックイベントを追加
                var i = wrapperIndex;
                icon.SetClickAction(() => SelectReward(i));
                
                wrapperIndex++;
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
                case StageType.Undefined:
                default:
                    throw new ArgumentOutOfRangeException(nameof(stageType), stageType, null);
            }
            
            // 報酬金額を表示
            rewardMoneyText.text = rewardMoney.ToString();
        }

        /**
         * 報酬を選択する
         */
        private void SelectReward(int index)
        {
            Debug.Log("SelectReward: " + index);
            
            if (_selectedRewardItemWrappers[index])
            {
                // 選択済みの報酬アイテムのラッパーのイメージを非表示
                rewardItemWrappers[index].GetComponent<Image>().enabled = false;
                
                // 選択済みの報酬アイテムのラッパーのインデックスを削除
                _selectedRewardItemWrappers[index] = false;
            }
            else
            {
                // trueの数が選択可能な報酬アイテムの数を超えている場合は選択できない
                if (_selectedRewardItemWrappers.Count(b => b) >= _selectableRewardAmount)
                    return;

                // 選択した報酬アイテムのラッパーのイメージを表示
                rewardItemWrappers[index].GetComponent<Image>().enabled = true;
                
                // 選択した報酬アイテムのラッパーのインデックスを追加
                _selectedRewardItemWrappers[index] = true;
            }
        }

        public void SubmitSelection()
        {
            _selectedRewardItem = new List<(ATrap, ATurret, ASkill)>();
            _selectedRewardType = new List<RewardType>();
            
            for (var index = 0; index < _selectedRewardItemWrappers.Count; index++)
            {
                // 選択済みか
                if (!_selectedRewardItemWrappers[index]) continue;
                
                // 選択済みの報酬アイテムを追加
                _selectedRewardItem.Add(_rewardItems[index]);
                _selectedRewardType.Add(_rewardTypes[index]);
            }
            
            invasionController.ReceiveReward(_rewardMoney, _selectedRewardItem, _selectedRewardType);
        }
    }
}
