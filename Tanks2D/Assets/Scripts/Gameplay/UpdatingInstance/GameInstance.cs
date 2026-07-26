using System.Collections.Generic;
using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.UpdatingInstance
{
    public class GameInstance : MonoBehaviour, IService
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly List<IUpdatable> _updatablesPauseIgnoring = new();
        private readonly List<IFixedUpdatable> _fixedUpdatables = new();
        private GameProgressor _gameProgressor;

        public void Start()
        {
            _gameProgressor = Services.Get<GameProgressor>();
        }

        private void Update()
        {
            for (int i = 0; i < _updatablesPauseIgnoring.Count; i++)
                _updatablesPauseIgnoring[i].OnUpdate();

            if (_gameProgressor.GameStatus != GameStatus.Running) return;
            for (int i = 0; i < _updatables.Count; i++)
                _updatables[i].OnUpdate();
        }

        private void FixedUpdate()
        {
            if (_gameProgressor.GameStatus != GameStatus.Running) return;
            for (int i = 0; i < _fixedUpdatables.Count; i++)
                _fixedUpdatables[i].OnFixedUpdate();
        }

        public void Register(IUpdatable updatable, bool ignorePause = false)
        {
            if (ignorePause)
            {
                if (!_updatablesPauseIgnoring.Contains(updatable))
                    _updatablesPauseIgnoring.Add(updatable);
                return;
            }

            if (!_updatables.Contains(updatable))
                _updatables.Add(updatable);
        }

        public void Unregister(IUpdatable updatable, bool ignorePause = false)
        {
            if (ignorePause)
            {
                if (_updatablesPauseIgnoring.Contains(updatable))
                    _updatablesPauseIgnoring.Remove(updatable);
                return;
            }

            if (_updatables.Contains(updatable))
                _updatables.Remove(updatable);
        }

        public void Register(IFixedUpdatable fixedupdatable)
        {
            if (!_fixedUpdatables.Contains(fixedupdatable))
                _fixedUpdatables.Add(fixedupdatable);
        }

        public void Unregister(IFixedUpdatable fixedupdatable)
        {
            if (_fixedUpdatables.Contains(fixedupdatable))
                _fixedUpdatables.Remove(fixedupdatable);
        }
    }
}