using System;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using TMPro;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    [SerializeField] private DefaultValueObject defaultValueObject;

    [SerializeField]
    private GeneralS2SData generalS2SData;
    
    [SerializeField]
    private TextMeshProUGUI walletText;

    public int Wallet { get; private set; }

    public void Start()
    {
        var saveData = SaveController.LoadWallet();
        var s2SData = generalS2SData.Wallet;
        
        if (saveData != -1)
        {
            Wallet = saveData;
        }
        else if (s2SData != -1)
        {
            Wallet = s2SData;
        }
        else
        {
            Wallet = defaultValueObject.defaultWallet;
        }
    }
    
    public void Update()
    {
        walletText.text = Wallet.ToString();
    }

    private void OnDestroy()
    {
        generalS2SData.Wallet = Wallet;
    }

    private void OnApplicationQuit()
    {
        SaveController.SaveWallet(Wallet);
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
        return Wallet >= price;
    }
}