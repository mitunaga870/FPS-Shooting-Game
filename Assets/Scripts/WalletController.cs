using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    [SerializeField] private DefaultValueObject defaultValueObject;

    public int Wallet { get; private set; }

    public void Start()
    {
        Wallet = defaultValueObject.defaultWallet;
    }

    /**
     * ウォレットにお金を追加
     */
    public void AddWallet(int amount)
    {
        // 負の数は追加しない
        if (amount < 0) return;

        Wallet += amount;
    }

    /**
     * ウォレットからお金を引く
     */
    public void SubtractWallet(int amount)
    {
        // 負の数は引かない
        if (amount < 0) return;

        // ウォレットにお金が足りない場合は0にする
        Wallet = Math.Max(0, Wallet - amount);
    }

    /**
     * 購入確認処理
     */
    public bool CanBuy(int price)
    {
        return price >= 0;
    }
}