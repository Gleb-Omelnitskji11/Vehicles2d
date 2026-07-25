using System;
using UnityEngine;

namespace Gameplay
{
    public class Timer : IUpdatable
    {
        private float _time;
        private Action _action;
        public bool IsRunning { get; private set; }

        public void SetupTimer(float time, Action action)
        {
            _action = action;
            _time = time;
            IsRunning = true;
        }

        public void OnUpdate()
        {
            if (!IsRunning) return;
            _time -= Time.deltaTime;

            if (_time <= 0)
            {
                _action?.Invoke();
                _action = null;
                IsRunning = false;
            }
        }

        public void Cancel()
        {
            IsRunning = false;
            _action = null;
        }
    }
}