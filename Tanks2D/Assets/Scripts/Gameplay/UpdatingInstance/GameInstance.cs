using System.Collections.Generic;
using Core.ServiceLocator;
using Enums;
using UnityEngine;

namespace Gameplay.UpdatingInstance
{
    public class GameInstance : MonoBehaviour, IService
    {
        public readonly List<IUpdatable> _updatables = new();
        public readonly List<IUpdatable> _updatablesPauseIgnoring = new();
        
        public readonly List<IUpdatable> _newSubForRegister = new();
        public readonly List<IUpdatable> _newSubForUnregister = new();
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
            if (!_newSubForRegister.Contains(updatable))
            {
                _newSubForRegister.Add(updatable);
                return true;
            }
            return false;
        }

        public void AddToUnsubUpdate(IUpdatable updatable)
        {
            if (!_newSubForUnregister.Contains(updatable))
            {
                _newSubForUnregister.Add(updatable);
            }
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