using System;

namespace Gameplay
{
    [Serializable]
    public class LevelData
    {
        public int EnemiesCount = 3;
        public int EnemiesMax = 2;

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