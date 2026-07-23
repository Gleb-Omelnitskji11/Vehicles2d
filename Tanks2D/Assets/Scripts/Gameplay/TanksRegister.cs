using System.Collections.Generic;
using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay
{
    public class TanksRegister : IService
    {
        public GameObject Player {get; private set;}
        private readonly List<GameObject> _activeEnemies = new List<GameObject>();
        
        public IReadOnlyList<GameObject> ActiveEnemies => _activeEnemies;

        public void RegisterPlayer(GameObject player)
        {
            Player = player;
        }

        public void RegisterEnemy(GameObject enemy)
        {
            _activeEnemies.Add(enemy);
        }
        
        public void UnRegisterEnemy(GameObject enemy)
        {
            _activeEnemies.Remove(enemy);
        }
    }
}