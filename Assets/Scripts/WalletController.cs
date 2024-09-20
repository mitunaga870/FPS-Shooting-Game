using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    [SerializeField] private DefaultValueObject defaultValueObject;

    private int wallet;

    public void Start()
    {
        wallet = defaultValueObject.defaultWallet;
    }
}