using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace DataClass
{
    [Serializable]
    public class TilePosition
    {
        public int Col;
        public int Row;

        public TilePosition(int row, int col)
        {
            Col = col;
            Row = row;
        }

        public TilePosition(Vector3 position, Vector3 origin)
        {
        }

        public override int GetHashCode()
        {
            return Col * Row;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var p = (TilePosition)obj;
            return Col == p.Col && Row == p.Row;
        }

        public bool Equals(TilePosition other)
        {
            return Col == other.Col && Row == other.Row;
        }

        public Vector3 ToVector3(Vector3 origin)
        {
            var relativePosition = new Vector3(Col, 0, Row);
            return relativePosition * Environment.TileSize + origin;
        }

        public override string ToString()
        {
            return $"Row: {Row}, Col: {Col}";
        }

        public TilePosition Rotate(double angle)
        {
            // ラジアンに変換
            angle = angle * Math.PI / 180;

            var x = Col * Math.Cos(angle) - Row * Math.Sin(angle);
            var y = Col * Math.Sin(angle) + Row * Math.Cos(angle);
            return new TilePosition((int)Math.Round(x), (int)Math.Round(y));
        }

        /**
         * 上のタイルを取得
         */
        public TilePosition GetUp()
        {
            return new TilePosition(Row - 1, Col);
        }

        /**
         * 下のタイルを取得
         */
        public TilePosition GetDown()
        {
            return new TilePosition(Row + 1, Col);
        }

        /**
         * 左のタイルを取得
         */
        public TilePosition GetLeft()
        {
            return new TilePosition(Row, Col - 1);
        }

        /**
         * 右のタイルを取得
         */
        public TilePosition GetRight()
        {
            return new TilePosition(Row, Col + 1);
        }

        /**
         * 右上のタイルを取得
         */
        public TilePosition GetRightUp()
        {
            return new TilePosition(Row - 1, Col + 1);
        }

        /**
         * 右下のタイルを取得
         */
        public TilePosition GetRightDown()
        {
            return new TilePosition(Row + 1, Col + 1);
        }

        /**
         * 左上のタイルを取得
         */
        public TilePosition GetLeftUp()
        {
            return new TilePosition(Row - 1, Col - 1);
        }

        /**
         * 左下のタイルを取得
         */
        public TilePosition GetLeftDown()
        {
            return new TilePosition(Row + 1, Col - 1);
        }

        public static float GetDistance(TilePosition enemyPosition, TilePosition setPosition)
        {
            return Mathf.Sqrt(
                Mathf.Pow(enemyPosition.Row - setPosition.Row, 2) +
                Mathf.Pow(enemyPosition.Col - setPosition.Col, 2)
            );
        }

        public float GetAngle(TilePosition targetPosition)
        {
            var x = targetPosition.Col - Col;
            var y = targetPosition.Row - Row;
            return Mathf.Atan2(y, x) * 180 / Mathf.PI;
        }
    }
}