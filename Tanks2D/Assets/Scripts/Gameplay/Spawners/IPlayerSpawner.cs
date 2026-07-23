using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay.Spawners
{
    public interface IPlayerSpawner : IService, IInit
    {
        GameObject SpawnPlayer();
    }
}