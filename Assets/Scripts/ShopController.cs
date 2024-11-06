using System;
using System.Collections;
using System.Collections.Generic;
using AClass;
using CreatePhase.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopController : MonoBehaviour
{
    public const int MAX_REROLL_COUNT = 1;
    public const int TRAP_COST = 50;
    public const int SKILL_COST = 100;
    public const int REROLL_COST = 20;

    // ショップを閉じるボタン
    [SerializeField] private Button closeShopButton;

    // リロールボタン
    [SerializeField] private Button ReRollButton;

    // 購入ボタン
    [SerializeField] private List<Button> trapButtons;
    [SerializeField] private List<Button> skillButtons;

    // 角商品の最大数
    public int TrapCount
    {
        get => trapButtons.Count;
    }

    public int SkillCount
    {
        get => skillButtons.Count;
    }
    
    //　ショップに並びうるものの一覧
    private List<ATrap> allTraps = new List<ATrap>();
    private List<ASkill> allSkills = new List<ASkill>();

    // ショップに並んでいるアイテム
    private List<ATrap> traps = new List<ATrap>();
    private List<ASkill> skills = new List<ASkill>();

    /** リロール回数 */
    private int reRollCount = 0;
    
    private DeckController deck;
    private WalletController wallet;

    public void Initialize(DeckController deckController, WalletController walletController)
    {
        // ショップフラグを立てる
        SaveController.SaveShopFlag(true);
            
        deck = deckController;
        wallet = walletController;
        
        // リソースフォルダから全てのトラップ、タレット、スキルを取得
        allTraps.AddRange(Resources.LoadAll<ATrap>("Prefabs/Traps"));
        allSkills.AddRange(Resources.LoadAll<ASkill>("Prefabs/Skills"));

        // SetItems();

        // ショップを閉じるボタンに処理を追加
        closeShopButton.onClick.AddListener(() =>
        {
            // ショップフラグを下げる
            SaveController.SaveShopFlag(false);
        });

        // リロールボタンに処理を追加
        ReRollButton.onClick.AddListener(() => { SetItems(true); });
    }

    /**
     * ショップにアイテムを並べる用の配列操作
     */
    public void SetItems(bool isReRoll = false)
    {
        // リロール回数が上限に達している場合は何もしない
        if (isReRoll && reRollCount >= MAX_REROLL_COUNT) return;

        // リロールの時の処理
        if (isReRoll)
        {
            // リロールコストが足りない場合はエラーを吐く
            if (!wallet.CanBuy(REROLL_COST)) throw new Exception("リロールコストが足りません");

            // ウォレットからリロールコストを引く
            wallet.SubtractWallet(REROLL_COST);

            // リロール回数を増やす
            reRollCount++;

            // リロール回数が上限に達している場合はリロールボタンを非アクティブにする
            if (reRollCount >= MAX_REROLL_COUNT) ReRollButton.interactable = false;
        }

        // トラップ
        traps.Clear();
        for (var i = 0; i < TrapCount; i++)
        {
            traps.Add(allTraps[Random.Range(0, allTraps.Count)]);

            var number = i;
            var trap = traps[i];

            // ボタンをアクティブに
            trapButtons[i].interactable = true;

            // ボタンのテキストを変更
            trapButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = trap.name + " " + TRAP_COST + " yen";

            // ボタンに購入処理を追加
            trapButtons[i].onClick.RemoveAllListeners();
            trapButtons[i].onClick.AddListener(() =>
            {
                Debug.Log("buy trap: " + trap.name + " for " + TRAP_COST + " yen");

                // お金が足りない場合は購入できない
                if (!wallet.CanBuy(TRAP_COST)) return;

                // ウォレットから購入金額を引く
                wallet.SubtractWallet(TRAP_COST);

                // デッキにトラップを追加
                deck.AddTrap(trap);

                Debug.Log("trap bought: " + trap.name + " for " + TRAP_COST + " yen");
                Debug.Log("after buy: " + wallet.Wallet + " yen");
                Debug.Log("deck traps: " + deck.TrapDeckCount);

                // 押せないようにインアクティブにする
                trapButtons[number].interactable = false;
            });
        }

        // スキル
        skills.Clear();

        if (skills.Count == 0) return;
        for (var i = 0; i < SkillCount; i++)
        {
            skills.Add(allSkills[Random.Range(0, allSkills.Count)]);

            // ボタンに購入処理を追加
            skillButtons[i].onClick.RemoveAllListeners();
            skillButtons[i].onClick.AddListener(() =>
            {
                // お金が足りない場合は購入できない
                if (!wallet.CanBuy(SKILL_COST)) return;

                // ウォレットから購入金額を引く
                wallet.SubtractWallet(SKILL_COST);

                // デッキにスキルを追加
                deck.AddSkill(skills[i]);
            });
        }
    }
    
    // ReSharper disable once InconsistentNaming
    public void SetOnClose(Action _onCloseAction)
    {
        closeShopButton.onClick.AddListener(_onCloseAction.Invoke);
    }
}