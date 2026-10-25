using System.Collections.Generic;
using Config;
using Core;
using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class EffectSpawner : MonoBehaviour, IService, IInit
    {
        [SerializeField] private Transform _effectParent;

        private readonly List<GameObject> _activeEffects = new List<GameObject>();

        private GameObject _hitEffectPrefab;
        private ProjectileConfigScript _projectileConfig;

        public bool IgnorePause { get; } = false;

        public void Init()
        {
            _projectileConfig = Services.Get<IConfigProvider>().ProjectileConfig;
        }

        public void SpawnBulletHitEffect(Vector2 position, ProjectileID projectileID)
        {
            _hitEffectPrefab = _projectileConfig.GetEffect(projectileID);
            if (_hitEffectPrefab == null)
                return;

            Instantiate(_hitEffectPrefab.gameObject, position, Quaternion.identity, _effectParent);
        }
    }
}
