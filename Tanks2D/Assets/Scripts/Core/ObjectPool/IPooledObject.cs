namespace Core.ObjectPool
{
    public interface IPooledObject
    {
        public bool IsActive { get;}
        public string PoolKey { get;}
        public IObjectPooler Pooler { get;}
        public void SetPoolData(IObjectPooler pooler, string poolKey);

        public void Deactivate();
        public void Activate();
    }
}