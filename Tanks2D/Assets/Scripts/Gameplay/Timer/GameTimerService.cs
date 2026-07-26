using System;
using System.Collections.Generic;
using Core.ServiceLocator;

namespace Gameplay.Timer
{
    public class GameTimerService : IService
    {
        private readonly List<Gameplay.Timer.Timer> _timers = new List<Gameplay.Timer.Timer>();

        public Gameplay.Timer.Timer AwaitGameOperation(float time, Action action)
        {
            foreach (var timer in _timers)
            {
                if(!timer.IsRunning)
                {
                    timer.SetupTimer(time, action);
                    return timer;
                }
            }

            Gameplay.Timer.Timer curTimer = CreateNewTimer();
            curTimer.SetupTimer(time, action);
            return curTimer;
        }

        private Gameplay.Timer.Timer CreateNewTimer()
        {
            var timer = new Gameplay.Timer.Timer();
            _timers.Add(timer);
            return timer;
        }
    }
}