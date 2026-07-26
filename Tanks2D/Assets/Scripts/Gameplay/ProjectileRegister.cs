using System.Collections.Generic;
using Core.ServiceLocator;
using Gameplay.Attacking;

namespace Gameplay
{
    public class ProjectileRegister : IService
    {
        private readonly List<BasicProjectile> _activeProjectile = new List<BasicProjectile>();
        
        public IReadOnlyList<BasicProjectile> ActiveProjectile => _activeProjectile;


        public void RegisterProjectile(BasicProjectile projectile)
        {
            _activeProjectile.Add(projectile);
        }
        
        public void UnRegisterProjectile(BasicProjectile projectile)
        {
            _activeProjectile.Remove(projectile);
        }
    }
}