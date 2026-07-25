using System;

namespace Core.ObjectPool
{
    public interface IPooledObject
    {
        public bool IsActive { get; }
        public string PoolKey { get; }
        public IObjectPooler Pooler { get; }
        public void SetPoolData(IObjectPooler pooler, string poolKey);

        public void Deactivate();
        public void Activate();
    }

    public class PooledObject<T> where T : class
    {
        private IObjectPooler _pooler;
        private string _poolKey;

        public void SetPoolableData(IObjectPooler pooler, string poolKey)
        {
            _pooler = pooler;
            _poolKey = poolKey;
        }
        
        public void Deactivate(T objectToSet, bool byManual)
        {
            _pooler.Release<T>(_poolKey, objectToSet );
        }
    }
}