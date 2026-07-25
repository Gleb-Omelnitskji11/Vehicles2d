using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay
{
    public class ProjectileRegister : IService
    {
        private readonly List<BasicProjectile> _activeProjectile = new List<BasicProjectile>();
        
        public IReadOnlyList<BasicProjectile> ActiveProjectile => _activeProjectile;


        public void RegisterProjectile(BasicProjectile projectile)
        {
            _activeProjectile.Add(projectile);
            projectile.Deactivated += UnRegisterProjectile;
        }
        
        public void UnRegisterProjectile(BasicProjectile projectile)
        {
            _activeProjectile.Remove(projectile);
        }
    }
}