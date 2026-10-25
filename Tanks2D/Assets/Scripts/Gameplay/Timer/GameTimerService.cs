using System;
using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay.Timer
{
    public class GameTimerService : IService
    {
        private readonly List<Timer> _timers = new List<Timer>();

        public Timer AwaitGameOperation(float time, Action action)
        {
            foreach (var timer in _timers)
            {
                if(!timer.IsRunning)
                {
                    timer.SetupTimer(time, action);
                    return timer;
                }
            }

            Timer curTimer = CreateNewTimer();
            curTimer.SetupTimer(time, action);
            return curTimer;
        }

        private Timer CreateNewTimer()
        {
            var timer = new Timer();
            _timers.Add(timer);
            return timer;
        }
    }
}