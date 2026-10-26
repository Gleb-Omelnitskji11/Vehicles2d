using System.Collections.Generic;
using Config;
using Core;
using Core.ObjectPool;
using Core.ServiceLocator;
using Enums;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay.Spawners
{
    public class EffectSpawner : MonoBehaviour, IService, IInit, IUpdatable
    {
        [SerializeField] private Transform _effectParent;

        private readonly List<GameObject> _activeEffects = new List<GameObject>();
        private readonly Dictionary<GameObject, ParticleSystem[]> _particleSystemsByEffect = new Dictionary<GameObject, ParticleSystem[]>();
        private readonly Dictionary<GameObject, string> _poolKeysByEffect = new Dictionary<GameObject, string>();
        private readonly Dictionary<ProjectileID, string> _poolKeysByProjectile = new Dictionary<ProjectileID, string>();

        private ProjectileConfigScript _projectileConfig;
        private IObjectPooler _pooler;
        private GameInstance _gameInstance;

        public bool IgnorePause => false;

        public void Init()
        {
            _projectileConfig = Services.Get<IConfigProvider>()
                .GetConfig<ProjectileConfigScript>(ConfigType.ProjectileConfig);
            _pooler = Services.Get<IObjectPooler>();
            _gameInstance = Services.Get<GameInstance>();

            if (_effectParent == null)
                _effectParent = transform;

            foreach (ProjectileID projectileID in System.Enum.GetValues(typeof(ProjectileID)))
            {
                GameObject prefab = _projectileConfig.GetEffect(projectileID);
                if (prefab == null)
                    continue;

                if (prefab.GetComponentInChildren<ParticleSystem>(true) == null)
                {
                    Debug.LogError($"[EffectSpawner] Effect for '{projectileID}' has no ParticleSystem.");
                    continue;
                }

                string poolKey = GetPoolKey(projectileID);
                _pooler.CreatePool<GameObject>(poolKey, () => CreateEffect(prefab, poolKey),
                    OnGetEffect, OnReleaseEffect);
                _poolKeysByProjectile.Add(projectileID, poolKey);
            }

            _gameInstance.TryAddNewUpdatable(this);
        }

        public void SpawnBulletHitEffect(Vector2 position, ProjectileID projectileID)
        {
            if (!_poolKeysByProjectile.TryGetValue(projectileID, out string poolKey))
                return;

            GameObject effect = _pooler.Get<GameObject>(poolKey);
            effect.transform.SetPositionAndRotation(position, Quaternion.identity);
            effect.SetActive(true);

            foreach (ParticleSystem particleSystem in _particleSystemsByEffect[effect])
            {
                if (particleSystem != null)
                    particleSystem.Play(true);
            }

            _activeEffects.Add(effect);
        }

        public void OnUpdate()
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                GameObject effect = _activeEffects[i];
                bool isAlive = false;

                foreach (ParticleSystem particleSystem in _particleSystemsByEffect[effect])
                {
                    if (particleSystem != null && particleSystem.IsAlive(true))
                    {
                        isAlive = true;
                        break;
                    }
                }

                if (isAlive)
                    continue;

                _activeEffects.RemoveAt(i);
                _pooler.Release(_poolKeysByEffect[effect], effect);
            }
        }

        private GameObject CreateEffect(GameObject prefab, string poolKey)
        {
            GameObject effect = Instantiate(prefab, _effectParent);
            _particleSystemsByEffect.Add(effect, effect.GetComponentsInChildren<ParticleSystem>(true));
            _poolKeysByEffect.Add(effect, poolKey);
            effect.SetActive(false);
            return effect;
        }

        private void OnGetEffect(GameObject effect)
        {
            foreach (ParticleSystem particleSystem in _particleSystemsByEffect[effect])
            {
                if (particleSystem != null)
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        private void OnReleaseEffect(GameObject effect)
        {
            foreach (ParticleSystem particleSystem in _particleSystemsByEffect[effect])
            {
                if (particleSystem != null)
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            effect.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_gameInstance != null)
                _gameInstance.AddToUnsubUpdate(this);
        }

        private static string GetPoolKey(ProjectileID projectileID) => $"HitEffect_{projectileID}";
    }
}
