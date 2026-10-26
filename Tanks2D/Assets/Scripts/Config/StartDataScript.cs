using Enums;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "StartDataConfig", menuName = "Configs/StartDataConfig")]
    public class StartDataScript : BaseConfigScript
    {
        [SerializeField] private VehicleType _startPlayerVehicle;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("_startPlayerprojectile")]
        private ProjectileID _startPlayerProjectile;

        public VehicleType StartPlayerVehicle => _startPlayerVehicle;
        public ProjectileID StartPlayerProjectile => _startPlayerProjectile;
        public override ConfigType Type => ConfigType.StartDataConfig;
    }
}
