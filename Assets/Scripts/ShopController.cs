using System;
using System.Collections;
using System.Collections.Generic;
using AClass;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ShopController : MonoBehaviour
{
    public const int TRAP_COUNT = 4;
    public const int SKILL_COUNT = 2;
    public const int MAX_REROLL_COUNT = 1;
    public const int TRAP_COST = 50;
    public const int SKILL_COST = 100;
    public const int REROLL_COST = 20;

    [SerializeField] private WalletController wallet;
    [SerializeField] private DeckController deck;

    /** トラップの購入ボタンが入るラッパーパネル */
    [SerializeField] private IPanel trapWrapperPanel;

    /** スキルの購入ボタンが入るラッパーパネル */
    [SerializeField] private IPanel skillWrapperPanel;

    /** リロールボタン */
    [SerializeField] private Button reRollButton;

    //　ショップに並びうるものの一覧
    private List<ATrap> allTraps = new List<ATrap>();
    private List<ASkill> allSkills = new List<ASkill>();

    // ショップに並んでいるアイテム
    private List<ATrap> traps = new List<ATrap>();
    private List<ASkill> skills = new List<ASkill>();

    /** リロール回数 */
    private int reRollCount = 0;

    public void Start()
    {
        // リソースフォルダから全てのトラップ、タレット、スキルを取得
        allTraps.AddRange(Resources.LoadAll<ATrap>("Prefabs/Traps"));
        allSkills.AddRange(Resources.LoadAll<ASkill>("Prefabs/Skills"));
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
        }

        // トラップ
        traps.Clear();
        for (var i = 0; i < TRAP_COUNT; i++)
        {
            traps.Add(allTraps[Random.Range(0, allTraps.Count)]);
        }

        // スキル
        skills.Clear();
        for (var i = 0; i < SKILL_COUNT; i++)
        {
            skills.Add(allSkills[Random.Range(0, allSkills.Count)]);
        }
    }
}