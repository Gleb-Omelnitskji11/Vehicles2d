using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay
{
    public class TanksRegister : IService
    {
        public Tank Player {get; private set;}
        private readonly List<Tank> _activeEnemies = new List<Tank>();
        
        public IReadOnlyList<Tank> ActiveEnemies => _activeEnemies;

        public void RegisterPlayer(Tank player)
        {
            Player = player;
        }

        public void RegisterEnemy(Tank enemy)
        {
            _activeEnemies.Add(enemy);
        }
        
        public void UnRegisterEnemy(Tank enemy)
        {
            _activeEnemies.Remove(enemy);
        }
    }
}