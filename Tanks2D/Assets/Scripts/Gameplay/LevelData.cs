using System;

namespace Gameplay
{
    [Serializable]
    public class LevelData
    {
        public int EnemiesCount = 5;
        public int EnemiesMax = 3;

        public LevelData GetCopy()
        {
            return new LevelData()
            {
                EnemiesCount = EnemiesCount,
                EnemiesMax = EnemiesMax,
            };
        }
    }
}