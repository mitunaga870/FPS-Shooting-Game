namespace DataClass
{
    public class TilePosition
    {
        public readonly int Col;
        public readonly int Row;

        public TilePosition(int row, int col)
        {
            Col = col;
            Row = row;
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

        protected bool Equals(TilePosition other)
        {
            return Col == other.Col && Row == other.Row;
        }
    }
}