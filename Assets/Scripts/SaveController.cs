using DataClass;
using Enums;
using JetBrains.Annotations;
using Map;
using ScriptableObjects;
using UnityEngine;

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

    public static void SaveStageData(StageData stageData)
    {
        PlayerPrefs.SetString("StageName", stageData.stageName);
        PlayerPrefs.SetInt("StageType", (int)stageData.StageType);
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
    public static StageData LoadStageData(StageObject stageObject)
    {
        // セーブデータがない場合はnullを返す
        var stageName = PlayerPrefs.GetString("StageName", null);
        if (stageName == "") return null;

        var result = stageObject.GetFromStageName(stageName);
        if (result == null) return null;

        // ステージタイプを読み込む
        result.StageType = (StageType)PlayerPrefs.GetInt("StageType", 0);

        // 読み込んだやつを消す
        PlayerPrefs.DeleteKey("StageName");
        PlayerPrefs.DeleteKey("StageType");

        return result;
    }

    [CanBeNull]
    public static MapWrapper[] LoadMap()
    {
        // TODO: 保存フォーマットを決めたらロード処理を書く（とりあえずnullを返す）
        return null;
    }

    /**
     * セーブデータを削除する
     */
    public static void DelSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public static (TrapData[] Traps, TurretData[] Turrets, SkillData[] Skills)? LoadDeckData()
    {
        // TODO: 保存フォーマットを決めたらロード処理を書く（とりあえずnullを返す）
        return null;
    }

    public static void SaveMap(MapWrapper[] mapWrappers)
    {
        // TODO: 保存フォーマットを決めたらセーブ処理を書く
    }
}