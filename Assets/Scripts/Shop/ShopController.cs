using System;
using System.Collections.Generic;
using AClass;
using TMPro;
using UI.Generator;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        private const int MAX_REROLL_COUNT = 1;
        // ReSharper disable once InconsistentNaming
        private const int TRAP_COST = 50;
        // ReSharper disable once InconsistentNaming
        private const int SKILL_COST = 100;
        // ReSharper disable once InconsistentNaming
        private const int REROLL_COST = 20;

        // ショップを閉じるボタン
        [SerializeField]
        private Button closeShopButton;

        // リロールボタン
        [SerializeField]
        private Button reRollButton;

    
        // アイコンのラッパーオブジェクト
        [SerializeField]
        private List<GameObject> trapIconWrappers;
        [SerializeField]
        private List<GameObject> skillIconWrappers;
        
        [FormerlySerializedAs("trapIconGenerator")]
        [SerializeField]
        private TrapCardGenerator trapCardGenerator;
        [FormerlySerializedAs("skillIconGenerator")]
        [SerializeField]
        private SkillCardGenerator skillCardGenerator;
        
        [SerializeField]
        private List<TextMeshProUGUI> trapCostTexts;
        [SerializeField]
        private List<TextMeshProUGUI> skillCostTexts;

        // 角商品の最大数
        private static int TrapCount => 3;

        private static int SkillCount => 2;
        public bool IsShopUIShowing => gameObject.activeSelf;

        //　ショップに並びうるものの一覧
        private readonly List<ATrap> _allTraps = new();
        private readonly List<ASkill> _allSkills = new();

        // ショップに並んでいるアイテム
        private readonly List<ATrap> _trapsOnSale = new();
        private readonly List<ASkill> _skillsOnSale = new();

        /** リロール回数 */
        private int _reRollCount;
    
        private DeckController _deck;
        private WalletController _wallet;

        public void Initialize(DeckController deckController, WalletController walletController)
        {
            // ショップフラグを立てる
            SaveController.SaveShopFlag(true);
            
            _deck = deckController;
            _wallet = walletController;
        
            // リソースフォルダから全てのトラップ、タレット、スキルを取得
            _allTraps.AddRange(Resources.LoadAll<ATrap>("Prefabs/Traps"));
            _allSkills.AddRange(Resources.LoadAll<ASkill>("Prefabs/Skill"));

            SetItems();

            // ショップを閉じるボタンに処理を追加
            closeShopButton.onClick.AddListener(() =>
            {
                // ショップフラグを下げる
                SaveController.SaveShopFlag(false);
            });

            // リロールボタンに処理を追加
            reRollButton.onClick.AddListener(() => { SetItems(true); });
        }

        /**
     * ショップにアイテムを並べる用の配列操作
     */
        public void SetItems(bool isReRoll = false)
        {
            // リロール回数が上限に達している場合は何もしない
            if (isReRoll && _reRollCount >= MAX_REROLL_COUNT) return;

            // リロールの時の処理
            if (isReRoll)
            {
                // リロールコストが足りない場合はエラーを吐く
                if (!_wallet.CanBuy(REROLL_COST)) throw new Exception("リロールコストが足りません");

                // ウォレットからリロールコストを引く
                _wallet.SubtractWallet(REROLL_COST);

                // リロール回数を増やす
                _reRollCount++;

                // リロール回数が上限に達している場合はリロールボタンを非アクティブにする
                if (_reRollCount >= MAX_REROLL_COUNT) reRollButton.interactable = false;
            }

            // トラップ
            _trapsOnSale.Clear();
            for (var i = 0; i < TrapCount; i++)
            {
                var number = i;
                var trap = _allTraps[Random.Range(0, _allTraps.Count)];
                var wrapper = trapIconWrappers[i];
                
                _trapsOnSale.Add(trap);
            
                // アイコン作成
                var trapIcon = trapCardGenerator.GetTrapIcon(trap.GetTrapName());
                trapIcon = Instantiate(trapIcon, wrapper.transform, false);

                // ボタンに購入処理を追加
                trapIcon.SetButtonAction(() =>
                {
                    Debug.Log("buy trap: " + trap.name + " for " + TRAP_COST + " yen");

                    // お金が足りない場合は購入できない
                    if (!_wallet.CanBuy(TRAP_COST)) return;

                    // ウォレットから購入金額を引く
                    _wallet.SubtractWallet(TRAP_COST);

                    // デッキにトラップを追加
                    _deck.AddTrap(trap);

                    Debug.Log("trap bought: " + trap.name + " for " + TRAP_COST + " yen");
                    Debug.Log("after buy: " + _wallet.Wallet + " yen");
                    Debug.Log("deck traps: " + _deck.TrapDeckCount);

                    // 押せないようにインアクティブにする
                    Destroy(trapIcon.gameObject);
                });
            }

            // スキル
            _skillsOnSale.Clear();

            for (var i = 0; i < SkillCount; i++)
            {
                var number = i;
                var skill = _allSkills[Random.Range(0, _allSkills.Count)];
                var wrapper = skillIconWrappers[i];
                
                _skillsOnSale.Add(skill);
                
                // アイコン作成
                var skillIcon = skillCardGenerator.GetSkillIcon(skill.GetSkillName());
                skillIcon = Instantiate(skillIcon, wrapper.transform, false);
                
                skillIcon.SetButtonAction(()=>
                {
                    Debug.Log("buy skill: " + skill.name + " for " + SKILL_COST + " yen");
                    
                    // お金が足りない場合は購入できない
                    if (!_wallet.CanBuy(SKILL_COST)) return;

                    // ウォレットから購入金額を引く
                    _wallet.SubtractWallet(SKILL_COST);

                    // デッキにスキルを追加
                    _deck.AddSkill(skill);
                    
                    Debug.Log("skill bought: " + skill.name + " for " + SKILL_COST + " yen");
                    Debug.Log("after buy: " + _wallet.Wallet + " yen");
                    Debug.Log("deck skills: " + _deck.SkillDeckCount);
                    
                    // 押せないように消す
                    Destroy(skillIcon.gameObject);
                });
            }
        }
    
        // ReSharper disable once InconsistentNaming
        public void SetOnClose(Action _onCloseAction)
        {
            closeShopButton.onClick.AddListener(_onCloseAction.Invoke);
        }
    }
}