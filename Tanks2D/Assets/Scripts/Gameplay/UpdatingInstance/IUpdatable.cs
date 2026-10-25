namespace Gameplay.UpdatingInstance
{
    public interface IUpdatable
    {
        bool IgnorePause { get; }
        void OnUpdate();
    }
}