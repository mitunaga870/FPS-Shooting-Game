using System;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class S2SDataInitializer : MonoBehaviour
{
    [SerializeField]
    private GeneralS2SData generalS2SData;

    [SerializeField]
    private CreateToInvasionData createToInvasionData;
    
    [SerializeField]
    private DefaultValueObject defaultValueObject;

    private void Awake()
    {
        if (generalS2SData.MapNumber == -1)
            generalS2SData.MapNumber = SaveController.LoadCurrentMapNumber();
        
        if (generalS2SData.CurrentMapRow == -1)
            generalS2SData.CurrentMapRow = SaveController.LoadCurrentMapRow();
        
        if (generalS2SData.CurrentMapColumn == -1)
            generalS2SData.CurrentMapColumn = SaveController.LoadCurrentMapColumn();
        
        if (generalS2SData.CurrentStageNumber == -1)
            generalS2SData.CurrentStageNumber = SaveController.LoadCurrentStageNumber();
        
        if (generalS2SData.PlayerHp == -1)
        {
            generalS2SData.PlayerHp = SaveController.LoadPlayerHP();
            
            if (generalS2SData.PlayerHp == -1)
                generalS2SData.PlayerHp = defaultValueObject.defaultPlayerHp;
        }
        
        if (generalS2SData.Wallet == -1)
        {
            generalS2SData.Wallet = SaveController.LoadWallet();
            
            if (generalS2SData.Wallet == -1)
                generalS2SData.Wallet = defaultValueObject.defaultWallet;
        }

        if (generalS2SData.Score == -1)
        {
            generalS2SData.Score = SaveController.LoadScore();
            
            if (generalS2SData.Score == -1)
                generalS2SData.Score = 0;
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    
    public void Save()
    {
        SaveController.SavePlayerHP(generalS2SData.PlayerHp);
        SaveController.SaveWallet(generalS2SData.Wallet);
    }
}