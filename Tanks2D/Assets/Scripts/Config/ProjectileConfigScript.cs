using System;
using Enums;
using Gameplay;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/ProjectileConfig")]
    public class ProjectileConfigScript : ScriptableObject
    {
        [SerializeField] private ProjectilePrefab[] _projectilePrefabs = new ProjectilePrefab[1];

        public BasicProjectile GetProjectilePrefab(ProjectileID projectileID)
        {
            foreach (var projectile in _projectilePrefabs)
            {
                if (projectile.ProjectileID == projectileID)
                {
                    return projectile.Prefab;
                }
            }
            
            Debug.LogError($"Projectile Prefab {projectileID} not found");
            throw new Exception("Projectile Prefab Not Found");
        }
        
        
        [Serializable]
        public class ProjectilePrefab
        {
            public ProjectileID ProjectileID;
            public BasicProjectile Prefab;
        }
    }
}