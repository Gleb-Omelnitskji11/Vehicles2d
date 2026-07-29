using System;
using Enums;

namespace Gameplay
{
    [Serializable]
    public class LevelData
    {
        public int Index = 1;
        public EnemyType[] Enemies = new EnemyType[5];
        public int EnemiesMax = 3;

        public LevelData GetCopy()
        {
            int enemiesCount = Enemies.Length;
            EnemyType[] enemies = new EnemyType[enemiesCount];
            Array.Copy(Enemies, 0, enemies, 0, enemiesCount);
            return new LevelData()
            {
                Index = Index,
                EnemiesMax = this.EnemiesMax,
                Enemies = enemies,
            };
        }
    }
}