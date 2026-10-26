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
        private readonly List<IUpdatable> _newSubForRegister = new();
        private readonly List<IUpdatable> _newSubForUnregister = new();
        private GameProgressor _gameProgressor;

        public void Start()
        {
            _gameProgressor = Services.Get<GameProgressor>();
        }

        private void Update()
        {
            UnsubOldUpdatable();
            RegisterNew();
            
            for (int i = 0; i < _updatablesPauseIgnoring.Count; i++)
                _updatablesPauseIgnoring[i].OnUpdate();

            if (_gameProgressor.GameStatus != GameStatus.Running) 
                return;
            
            for (int i = 0; i < _updatables.Count; i++)
                _updatables[i].OnUpdate();
        }
        
        public bool TryAddNewUpdatable(IUpdatable updatable)
        {
            if (updatable == null)
                return false;

            _newSubForUnregister.Remove(updatable);
            if (_updatables.Contains(updatable) || _updatablesPauseIgnoring.Contains(updatable) ||
                _newSubForRegister.Contains(updatable))
                return false;

            _newSubForRegister.Add(updatable);
            return true;
        }

        public void AddToUnsubUpdate(IUpdatable updatable)
        {
            if (updatable == null)
                return;

            _newSubForRegister.Remove(updatable);
            if ((_updatables.Contains(updatable) || _updatablesPauseIgnoring.Contains(updatable)) &&
                !_newSubForUnregister.Contains(updatable))
                _newSubForUnregister.Add(updatable);
        }
        
        private void RegisterNew()
        {
            foreach (var updatable in _newSubForRegister)
            {
                if (updatable.IgnorePause)
                {
                    if (!_updatablesPauseIgnoring.Contains(updatable))
                        _updatablesPauseIgnoring.Add(updatable);
                    continue;
                }

                if (!_updatables.Contains(updatable))
                    _updatables.Add(updatable);
            }
            
            _newSubForRegister.Clear();
        }
        
        
        private void UnsubOldUpdatable()
        {
            foreach (var updatable in _newSubForUnregister)
            {
                if (updatable.IgnorePause)
                {
                    if (_updatablesPauseIgnoring.Contains(updatable))
                        _updatablesPauseIgnoring.Remove(updatable);
                    continue;
                }

                if (_updatables.Contains(updatable))
                    _updatables.Remove(updatable);
            }
            
            _newSubForUnregister.Clear();
        }
    }
}
