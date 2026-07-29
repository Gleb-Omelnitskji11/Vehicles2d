using System;
using Gameplay.UpdatingInstance;
using UnityEngine;

namespace Gameplay.Timer
{
    public class Timer : IUpdatable
    {
        public float Time {get; private set;}
        private Action _action;
        public bool IsRunning { get; private set; }

        public void SetupTimer(float time, Action action)
        {
            _action = action;
            Time = time;
            IsRunning = true;
        }

        public void OnUpdate()
        {
            if (!IsRunning) return;
            Time -= UnityEngine.Time.deltaTime;

            if (Time <= 0)
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