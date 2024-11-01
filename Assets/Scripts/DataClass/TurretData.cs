using AClass;

namespace DataClass
{
    public class TurretData
    {
        public readonly int Column;
        public readonly int Row;
        public readonly string Turret;
        public readonly int angle;

        public TurretData(int row, int column, ATurret turret)
        {
            Row = row;
            Column = column;
            Turret = turret.GetTurretName();
            angle = turret.Angle;
        }

        public TurretData(int row, int column, string turretName, int turretAngle)
        {
            Row = row;
            Column = column;
            Turret = turretName;
            angle = turretAngle;
        }

        public TurretData(string turretData)
        {
            var data = turretData.Split("%%");

            Row = int.Parse(data[0]);
            Column = int.Parse(data[1]);
            Turret = data[2];
            angle = int.Parse(data[3]);
        }

        public TurretData DriveAngle(int angle)
        {
            return new TurretData(
                Row, Column, Turret, angle
            );
        }

        public override string ToString()
        {
            return $"{Row}%%{Column}%%{Turret}%%{angle}";
        }
    }
}