using System.Threading.Tasks;
using Config;
using Core.ServiceLocator;

namespace Core
{
    public interface IConfigProvider : IService
    {
        Task LoadConfigAsync();
        TankConfigScript TankConfig { get; }
        ProjectileConfigScript ProjectileConfig { get; }
    }
}