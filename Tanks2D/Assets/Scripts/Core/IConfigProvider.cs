using System.Threading.Tasks;
using Config;

namespace Core
{
    public interface IConfigProvider
    {
        Task<TankConfigScript> LoadConfigAsync();
        TankConfigScript TankConfig { get; }
    }
}