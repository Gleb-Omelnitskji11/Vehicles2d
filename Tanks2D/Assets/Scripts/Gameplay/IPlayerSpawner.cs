using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay
{
    public interface IPlayerSpawner : IService, IInit
    {
        GameObject Spawn();
    }
}