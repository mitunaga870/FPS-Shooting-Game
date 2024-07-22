using DataClass;
using UnityEngine;

public static class SaveControler
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

            saveText += "\n";
        }

        PlayerPrefs.SetString("TileData", saveText);
    }

    // =======　読み込み処理　=======
    public static TileData[][] LoadTileData()
    {
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
}