using Core.ServiceLocator;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class PlayerFinder : IPlayerFinder
    {
        private TanksRegister _tanksRegister;
        private LayerMask _obstacleMask;
        private Transform _bot;
        private float _playerViewDistance;
        private float _viewAngle;

        public PlayerFinder(Transform bot, LayerMask obstacleMask, float playerViewDistance = 16f, float viewAngle = 45f)
        {
            _tanksRegister = Services.Get<TanksRegister>();
            _bot = bot;
            _obstacleMask = obstacleMask;

            _playerViewDistance = playerViewDistance;
            _viewAngle = viewAngle;
        }

        public bool TryFind(ref Transform player)
        {
            Vector2 toPlayer = _tanksRegister.Player.transform.position - _bot.position;

            if (toPlayer.sqrMagnitude <= _playerViewDistance * _playerViewDistance)
            {
                float angle = Vector2.Angle(_bot.up, toPlayer);

                if (angle <= _viewAngle)
                {
                    RaycastHit2D hit = Physics2D.Raycast(_bot.position, toPlayer.normalized,
                        toPlayer.magnitude, _obstacleMask);

                    if (hit.collider == null)
                    {
                        player = _tanksRegister.Player.transform;
                        return true;
                    }
                }
            }

            player = null;
            return false;
        }
    }
}