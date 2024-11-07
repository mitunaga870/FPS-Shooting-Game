using System;
using System.Collections.Generic;
using AClass;
using DataClass;
using Enums;
using JetBrains.Annotations;
using lib;
using Map;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR;

public static class SaveController
{
    // =======　保存処理　=======
    public static void SaveTileData(TileData[][] tileData)
    {
        // Save tileData like CSV
        var saveText = "";

        foreach (var row in tileData)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var tile in row) saveText += $"{tile},";

            // 最後のカンマを削除
            saveText = saveText.Substring(0, saveText.Length - 1);

            saveText += "\n";
        }

        // 最後の空行を削除
        saveText = saveText.Substring(0, saveText.Length - 1);

        PlayerPrefs.SetString("TileData", saveText);
    }

    public static void SavePhase(Phase phase)
    {
        PlayerPrefs.SetInt("Phase", (int)phase);
    }

    public static void SaveTrapData(TrapData[] trapData)
    {
        // Save trapData like CSV
        var saveText = "";

        foreach (var trap in trapData) saveText += $"{trap},";

        // 最後のカンマを削除
        saveText = saveText.Substring(0, saveText.Length - 1);

        PlayerPrefs.SetString("TrapData", saveText);
    }

    public static void SaveTurretData(List<TurretData> turretData)
    {
        // Save trapData like CSV
        var saveText = "";

        foreach (var turret in turretData) saveText += $"{turret},";

        if (saveText.Length == 0)
        {
            PlayerPrefs.SetString("TurretData", "");
            return;
        }

        // 最後のカンマを削除
        saveText = saveText.Substring(0, saveText.Length - 1);

        PlayerPrefs.SetString("TurretData", saveText);
    }

    public static void SaveStageData(StageData stageData)
    {
        PlayerPrefs.SetString("StageName", stageData.stageName);
        PlayerPrefs.SetInt("StageType", (int)stageData.StageType);
        PlayerPrefs.SetString("CustomData", stageData.StageCustomData.ToString());
    }

    public static void SaveMap(MapWrapper[] mapWrappers)
    {
        var saveText = "";
        for (var i = 0; i < mapWrappers.Length; i++)
        {
            saveText += mapWrappers[i].ToString();
            if (i != mapWrappers.Length - 1) saveText += "\n\n";
        }

        PlayerPrefs.SetString("Map", saveText);
    }

    public static void SaveCurrentMapNumber(int mapNumber)
    {
        PlayerPrefs.SetInt("MapNumber", mapNumber);
    }

    public static void SaveCurrentMapRow(int currentMapRow)
    {
        PlayerPrefs.SetInt("CurrentMapRow", currentMapRow);
    }

    public static void SaveCurrentMapColumn(int currentMapColumn)
    {
        PlayerPrefs.SetInt("CurrentMapColumn", currentMapColumn);
    }
    
    public static void SaveShopFlag(bool openShop)
    {
        PlayerPrefs.SetInt("OpenShop", openShop ? 1 : 0);
    }
    
    public static void SaveWallet(int wallet)
    {
        PlayerPrefs.SetInt("Wallet", wallet);
    }
    
    public static void SaveDeckData(
        ATrap[] deckTraps, ATrap[] handTraps, ATrap[] discardTraps,
        ASkill[] aSkills, ATurret[] aTurrets)
    {
        // 名前で行ごとに分ける
        var saveText = "";
        
        // トラップ山札
        foreach (var trap in deckTraps) saveText += $"{trap},";
        saveText = saveText.Substring(0, saveText.Length - 1);
        saveText += "\n";
        
        // トラップ手札
        foreach (var trap in handTraps) saveText += $"{trap},";
        saveText = saveText.Substring(0, saveText.Length - 1);
        saveText += "\n";
        
        // トラップ捨て場
        foreach (var trap in discardTraps) saveText += $"{trap},";
        saveText = saveText.Substring(0, saveText.Length - 1);
        saveText += "\n";
        
        // スキル
        foreach (var skill in aSkills) saveText += $"{skill},";
        saveText = saveText.Substring(0, saveText.Length - 1);
        saveText += "\n";
        
        // タレット
        foreach (var turret in aTurrets) saveText += $"{turret},";
        saveText = saveText.Substring(0, saveText.Length - 1);
        
        PlayerPrefs.SetString("DeckData", saveText);
    }

    // =======　読み込み処理　=======
    [CanBeNull]
    public static TileData[][] LoadTileData()
    {
        // セーブデータがない場合はnullを返す
        if (!PlayerPrefs.HasKey("TileData")) return null;

        // セーブデータを読み込む
        var saveText = PlayerPrefs.GetString("TileData");

        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("TileData");

        // セーブデータをCSV形式で読み込む
        // Load tileData like CSV
        var rows = saveText.Split('\n');
        var tileData = new TileData[rows.Length][];

        for (var i = 0; i < rows.Length; i++)
        {
            var tiles = rows[i].Split(',');
            tileData[i] = new TileData[tiles.Length];

            for (var j = 0; j < tiles.Length; j++) tileData[i][j] = new TileData(tiles[j]);
        }

        return tileData;
    }

    public static Phase LoadPhase()
    {
        var phase = PlayerPrefs.GetInt("Phase", 0);
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("Phase");
        return (Phase)phase;
    }

    public static TrapData[] LoadTrapData()
    {
        if (!PlayerPrefs.HasKey("TrapData")) return null;

        var saveText = PlayerPrefs.GetString("TrapData");
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("TrapData");

        // Load trapData like CSV
        var traps = saveText.Split(',');
        var trapData = new TrapData[traps.Length];

        for (var i = 0; i < traps.Length; i++) trapData[i] = new TrapData(traps[i]);

        return trapData;
    }

    [CanBeNull]
    public static TurretData[] LoadTurretData()
    {
        if (!PlayerPrefs.HasKey("TurretData")) return null;

        var saveText = PlayerPrefs.GetString("TurretData");
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("TurretData");
        
        // セーブデータが空文字列の場合は空の配列を返す
        if (string.IsNullOrEmpty(saveText))
            return Array.Empty<TurretData>();

        // Load trapData like CSV
        var turrets = saveText.Split(',');
        var turretData = new TurretData[turrets.Length];

        for (var i = 0; i < turrets.Length; i++) turretData[i] = new TurretData(turrets[i]);

        return turretData;
    }

    [CanBeNull]
    public static StageData LoadStageData(StageObject stageObject)
    {
        // セーブデータがない場合はnullを返す
        var stageName = PlayerPrefs.GetString("StageName", null);
        if (stageName == "") return null;

        var result = stageObject.GetFromStageName(stageName);
        if (result == null) return null;

        // ステージタイプを読み込む
        result.StageType = (StageType)PlayerPrefs.GetInt("StageType", 0);
        
        // カスタムデータを読み込む
        var customData = PlayerPrefs.GetString("CustomData", null);
        if (customData != null) result.StageCustomData = new StageCustomData(customData);

        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("StageName");
        PlayerPrefs.DeleteKey("StageType");
        PlayerPrefs.DeleteKey("CustomData");

        return result;
    }

    [CanBeNull]
    public static MapWrapper[] LoadMap()
    {
        // キーを持ってないならヌルを返す
        if (!PlayerPrefs.HasKey("Map")) return null;

        // 値を取得
        var saveText = PlayerPrefs.GetString("Map", null);
        // 取得したものを放棄
        PlayerPrefs.DeleteKey("Map");

        if (string.IsNullOrEmpty(saveText))
            return null;

        // 二十改行で分割
        var mapData = saveText.Split(new[] { "\n\n" }, StringSplitOptions.None);
        var mapWrappers = new MapWrapper[mapData.Length];

        for (var i = 0; i < mapData.Length; i++) mapWrappers[i] = new MapWrapper(mapData[i]);

        return mapWrappers;
    }

    public static int LoadCurrentMapNumber()
    {
        var mapNumber = PlayerPrefs.GetInt("MapNumber", 0);
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("MapNumber");
        return mapNumber;
    }

    public static int LoadCurrentMapRow()
    {
        var currentMapRow = PlayerPrefs.GetInt("CurrentMapRow", 0);
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("CurrentMapRow");
        return currentMapRow;
    }

    public static int LoadCurrentMapColumn()
    {
        var currentMapColumn = PlayerPrefs.GetInt("CurrentMapColumn", 0);
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("CurrentMapColumn");
        return currentMapColumn;
    }
    
    public static bool LoadShopFlag()
    {
        var openShop = PlayerPrefs.GetInt("OpenShop", 0);
        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("OpenShop");
        return openShop == 1;
    }

    /**
     * セーブデータを削除する
     */
    public static void DelSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public static (
        ATrap[] DeckTraps, ATrap[] HandTraps, ATrap[] DiscardTraps,
        ATurret[] Turrets, ASkill[] Skills)? LoadDeckData()
    {
        if (!PlayerPrefs.HasKey("DeckData")) return null;
        
        // セーブデータを読み込んで消す
        var saveText = PlayerPrefs.GetString("DeckData");
        PlayerPrefs.DeleteKey("DeckData");
        
        if (saveText == "") return null;
        
        // セーブデータを行ごとに分ける
        var rows = saveText.Split('\n');
        
        // 山札トラップ
        var traps = rows[0].Split(',');
        var trapData = new ATrap[traps.Length];
        for (var i = 0; i < traps.Length; i++) trapData[i] = InstanceGenerator.GenerateTrap(traps[i]);
        
        // 手札トラップ
        traps = rows[1].Split(',');
        var handTrapData = new ATrap[traps.Length];
        for (var i = 0; i < traps.Length; i++) handTrapData[i] = InstanceGenerator.GenerateTrap(traps[i]);
        
        // 捨て場トラップ
        traps = rows[2].Split(',');
        var discardTrapData = new ATrap[traps.Length];
        for (var i = 0; i < traps.Length; i++) discardTrapData[i] = InstanceGenerator.GenerateTrap(traps[i]);
        
        // スキル
        var skills = rows[3].Split(',');
        var skillData = new ASkill[skills.Length];
        for (var i = 0; i < skills.Length; i++) skillData[i] = InstanceGenerator.GenerateSkill(skills[i]);
        
        // タレット
        var turrets = rows[4].Split(',');
        var turretData = new ATurret[turrets.Length];
        for (var i = 0; i < turrets.Length; i++) turretData[i] = InstanceGenerator.GenerateTurret(turrets[i]);
        
        return (trapData, handTrapData, discardTrapData, turretData, skillData);
    }

    public static int LoadWallet()
    {
        return PlayerPrefs.GetInt("Wallet", -1);
    }
}