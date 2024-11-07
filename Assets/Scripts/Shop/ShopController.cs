using System;
using System.Collections.Generic;
using AClass;
using TMPro;
using UnityEngine;
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

        // 購入ボタン
        [SerializeField]
        private List<Button> trapButtons;
        [SerializeField]
        private List<Button> skillButtons;
    
        // アイコンのラッパーオブジェクト
        [SerializeField]
        private List<GameObject> trapIconWrappers;
        [SerializeField]
        private List<GameObject> skillIconWrappers;
        
        [SerializeField]
        private ShopTrapIconGenerator trapIconGenerator;
        [SerializeField]
        private ShopSkillIconGenerator skillIconGenerator;
        
        [SerializeField]
        private List<TextMeshProUGUI> trapCostTexts;
        [SerializeField]
        private List<TextMeshProUGUI> skillCostTexts;

        // 角商品の最大数
        private int TrapCount => trapButtons.Count;

        private int SkillCount => skillButtons.Count;

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
                _trapsOnSale.Add(_allTraps[Random.Range(0, _allTraps.Count)]);

                var number = i;
                var trap = _trapsOnSale[i];
                var wrapper = trapIconWrappers[i];
            
                // アイコン作成
                var trapIcon = trapIconGenerator.GetTrapIcon(trap.GetTrapName());
                trapIcon = Instantiate(trapIcon, wrapper.transform, false);

                // ボタンをアクティブに
                trapButtons[i].interactable = true;
                
                // コスト表示
                trapCostTexts[i].text = TRAP_COST.ToString();

                // ボタンに購入処理を追加
                trapButtons[i].onClick.RemoveAllListeners();
                trapButtons[i].onClick.AddListener(() =>
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
                    trapButtons[number].interactable = false;
                });
            }

            // スキル
            _skillsOnSale.Clear();

            for (var i = 0; i < SkillCount; i++)
            {
                _skillsOnSale.Add(_allSkills[Random.Range(0, _allSkills.Count)]);
                
                var skill = _skillsOnSale[i];
                var wrapper = skillIconWrappers[i];
                
                // アイコン作成
                var skillIcon = skillIconGenerator.GetSkillIcon(skill.GetSkillName());
                skillIcon = Instantiate(skillIcon, wrapper.transform, false);
                
                // ボタンをアクティブに
                skillButtons[i].interactable = true;
                
                // コスト表示
                skillCostTexts[i].text = SKILL_COST.ToString();

                // ボタンに購入処理を追加
                skillButtons[i].onClick.RemoveAllListeners();
                skillButtons[i].onClick.AddListener(() =>
                {
                    // お金が足りない場合は購入できない
                    if (!_wallet.CanBuy(SKILL_COST)) return;

                    // ウォレットから購入金額を引く
                    _wallet.SubtractWallet(SKILL_COST);

                    // デッキにスキルを追加
                    _deck.AddSkill(skill);
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