using System;
using Enums;
using Gameplay;
using Gameplay.Attacking;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/ProjectileConfig")]
    public class ProjectileConfigScript : ScriptableObject
    {
        [SerializeField] private ProjectileData[] _projectileData= new ProjectileData[1];

        public BasicProjectile GetProjectilePrefab(ProjectileID projectileID)
        {
            foreach (var projectileData in _projectileData)
            {
                if (projectileData.ProjectileID == projectileID)
                {
                    return projectileData.Prefab;
                }
            }
            
            Debug.LogError($"Projectile Data {projectileID} not found");
            throw new Exception("Projectile Data Not Found");
        }
        
        
        [Serializable]
        public class ProjectileData
        {
            public ProjectileID ProjectileID;
            public BasicProjectile Prefab;
        }
    }
}