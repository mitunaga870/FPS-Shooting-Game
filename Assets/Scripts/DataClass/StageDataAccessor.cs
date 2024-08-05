using ScriptableObjects;

namespace DataClass
{
    public class StageDataAccessor
    {
        public readonly int Stage;
        public readonly int Level;
        public readonly int ReRollWaitTime;
        public readonly int MazeRows;
        public readonly int TrapCount;
        public readonly int MazeColumns;
        public readonly TilePosition StartPosition;
        public readonly TilePosition GoalPosition;

        public StageDataAccessor(StageData stageData, int stage, int level)
        {
            Stage = stage;
            Level = level;

            if (stage == 1 && level == 1)
            {
                ReRollWaitTime = stageData.OneOneReRollWaitTime;
                MazeRows = stageData.OneOneMazeRows;
                MazeColumns = stageData.OneOneMazeColumns;
                TrapCount = stageData.OneOneTrapCount;
                StartPosition = new TilePosition(stageData.OneOneStartRow, stageData.OneOneStartColumn);
                GoalPosition = new TilePosition(stageData.OneOneGoalRow, stageData.OneOneGoalColumn);
            }
        }
    }
}