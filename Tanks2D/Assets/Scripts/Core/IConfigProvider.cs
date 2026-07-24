using Config;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IConfigProvider : IService
    {
        UniTask LoadConfigAsync();
        TankConfigScript TankConfig { get; }
        ProjectileConfigScript ProjectileConfig { get; }
    }
}