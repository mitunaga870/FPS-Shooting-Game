using System;
using UnityEngine.UIElements;

namespace DataClass
{
    /**
     * 経路情報を保持するクラス
     */
    public class Path
    {
        private readonly int[] _columns;
        private readonly int[] _rows;

        public Path()
        {
            _columns = Array.Empty<int>();
            _rows = Array.Empty<int>();
        }

        public Path(TilePosition position)
        {
            _columns = new[] { position.Col };
            _rows = new[] { position.Row };
        }

        public Path(int[] rows, int[] columns)
        {
            this._columns = columns;
            this._rows = rows;

            // パスなので、columnsとrowsの長さは同じである必要がある
            if (columns.Length != rows.Length)
            {
                throw new System.ArgumentException("columns and rows must have the same length");
            }
        }

        /**
         * パスを追加する
         * イミュータブルにする為に新しいPathを返す
         * int版
         */
        public Path Add(int row, int col)
        {
            var newColumns = new int[_columns.Length + 1];
            var newRows = new int[_rows.Length + 1];
            Array.Copy(_columns, newColumns, _columns.Length);
            Array.Copy(_rows, newRows, _rows.Length);
            newColumns[_columns.Length] = col;
            newRows[_rows.Length] = row;
            return new Path(newColumns, newRows);
        }

        /**
         * パスを追加する
         * イミュータブルにする為に新しいPathを返す
         * TilePosition版
         */
        public Path Add(TilePosition position)
        {
            return Add(position.Row, position.Col);
        }

        /**
         * パスに指定した位置が含まれているか
         * TilePosition版
         */
        public bool Contains(TilePosition position)
        {
            return Contains(position.Row, position.Col);
        }

        /**
         * パスに指定した位置が含まれているか
         * int版
         */
        public bool Contains(int row, int col)
        {
            // パスの中に指定した位置が含まれているか
            for (var i = 0; i < Length(); i++)
            {
                if (_columns[i] == col && _rows[i] == row)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * パスの長さを取得する
         */
        public int Length()
        {
            return _columns.Length;
        }

        public TilePosition GetLast()
        {
            return new TilePosition(_columns[Length() - 1], _rows[Length() - 1]);
        }
    }
}