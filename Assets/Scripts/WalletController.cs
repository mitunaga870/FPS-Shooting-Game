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
}