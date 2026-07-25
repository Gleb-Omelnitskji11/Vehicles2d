using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay
{
    public class TanksRegister : IService
    {
        public Vehicle Player {get; private set;}
        private readonly List<Vehicle> _activeTanks = new List<Vehicle>();
        
        public IReadOnlyList<Vehicle> ActiveTanks => _activeTanks;
        
        
        public void RegisterPlayer(Vehicle player)
        {
            Player = player;
        }
        
        public void UnRegisterPlayer()
        {
            Player = null;
        }

        public void RegisterVehicle(Vehicle tank)
        {
            _activeTanks.Add(tank);
        }
        
        public void UnRegisterVehicle(Vehicle tank)
        {
            _activeTanks.Remove(tank);
        }
    }
}