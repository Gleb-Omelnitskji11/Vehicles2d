namespace Gameplay
{
    public class LevelLoader : ILevelLoader
    {
        public LevelData LoadLevel(int levelIndex)
        {
            var level = new LevelData();
            return level.GetCopy();
        }
    }
}