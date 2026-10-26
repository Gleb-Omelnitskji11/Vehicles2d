using Config;
using Core.ServiceLocator;
using Cysharp.Threading.Tasks;
using Enums;

namespace Core
{
    public interface IConfigProvider : IService
    {
        UniTask LoadConfigAsync();
        T GetConfig<T>(ConfigType type) where T : BaseConfigScript;
    }
}
