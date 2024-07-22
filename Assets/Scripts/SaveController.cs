using DataClass;
using Enums;
using JetBrains.Annotations;
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
            foreach (var tile in row)
            {
                saveText += $"{tile},";
            }

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

    // =======　読み込み処理　=======
    [CanBeNull]
    public static TileData[][] LoadTileData()
    {
        if (!PlayerPrefs.HasKey("TileData")) return null;

        var saveText = PlayerPrefs.GetString("TileData");

        // Load tileData like CSV
        var rows = saveText.Split('\n');
        var tileData = new TileData[rows.Length][];

        for (var i = 0; i < rows.Length; i++)
        {
            var tiles = rows[i].Split(',');
            tileData[i] = new TileData[tiles.Length];

            for (var j = 0; j < tiles.Length; j++)
            {
                tileData[i][j] = new TileData(tiles[j]);
            }
        }

        return tileData;
    }

    public static Phase LoadPhase()
    {
        return (Phase)PlayerPrefs.GetInt("Phase", 0);
    }

    /**
     * セーブデータを削除する
     */
    public static void DelSave()
    {
        PlayerPrefs.DeleteAll();
    }
}