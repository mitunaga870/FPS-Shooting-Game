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
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

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
    }
}