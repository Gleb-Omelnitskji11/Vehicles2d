using UnityEngine;

namespace Gameplay.Enemy
{
    public interface IPlayerFinder
    {
        bool TryFind(ref Transform player);
    }
}