using Config;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IConfigProvider : IService
    {
        UniTask LoadConfigAsync();
        VehicleConfigScript VehicleConfig { get; }
        ProjectileConfigScript ProjectileConfig { get; }
    }
}