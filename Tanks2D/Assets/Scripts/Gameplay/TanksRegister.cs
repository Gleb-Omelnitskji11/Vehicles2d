using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay
{
    public class TanksRegister : IService
    {
        private readonly List<Vehicle> _activeTanks = new List<Vehicle>();
        
        public IReadOnlyList<Vehicle> ActiveTanks => _activeTanks;


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