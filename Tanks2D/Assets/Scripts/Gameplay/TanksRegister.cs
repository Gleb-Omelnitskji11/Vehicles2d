using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay
{
    public class TanksRegister : IService
    {
        private readonly List<Tank> _activeTanks = new List<Tank>();
        
        public IReadOnlyList<Tank> ActiveTanks => _activeTanks;


        public void RegisterTank(Tank tank)
        {
            _activeTanks.Add(tank);
        }
        
        public void UnRegisterTank(Tank tank)
        {
            _activeTanks.Remove(tank);
        }
    }
}