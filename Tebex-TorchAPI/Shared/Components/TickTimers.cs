using System;
using System.Collections.Generic;
using System.Linq;
using Tebex.Adapters;

namespace Tebex.Shared.Components
{
    /// <summary>
    /// TickTimers is an implementation of executing a scheduled action using a tick-based update method.
    /// </summary>
    public class TickTimers
    {
        private static BaseTebexAdapter _adapter;
        public TickTimers(BaseTebexAdapter adapter)
        {
            _adapter = adapter;
        }
        
        private class ScheduledAction
        {
            public DateTime NextExecutionTime;
            public TimeSpan Interval;
            public Action Action;
            public bool Repeat;
            
            public ScheduledAction(DateTime nextExecutionTime, TimeSpan interval, Action action, bool repeat)
            {
                NextExecutionTime = nextExecutionTime;
                Interval = interval;
                Action = action;
                Repeat = repeat;
            }
        }

        private readonly List<ScheduledAction> _scheduledActions = new List<ScheduledAction>();

        /// <summary>
        /// Updates the scheduled actions, executing any that are due.
        /// This method should be called on every tick.
        /// </summary>
        public void Update()
        {
            var now = DateTime.UtcNow;
            var actionsToExecute = _scheduledActions.Where(a => a.NextExecutionTime <= now).ToList();

            foreach (var action in actionsToExecute)
            {
                try
                {
                    action.Action();
                }
                catch (Exception ex)
                {
                    _adapter.LogError("Error executing scheduled action");
                    _adapter.LogError(ex.Message);
                    _adapter.LogError(ex.StackTrace);
                }
                finally
                {
                    _scheduledActions.Remove(action);
                }

                if (action.Repeat)
                {
                    action.NextExecutionTime += action.Interval;
                    _scheduledActions.Add(action);
                }
            }
        }

        /// <summary>
        /// Schedules an action to be executed repeatedly at a fixed interval.
        /// </summary>
        public void Every(double intervalInSeconds, Action action)
        {
            var interval = TimeSpan.FromSeconds(intervalInSeconds);
            var nextExecutionTime = DateTime.UtcNow + interval;
            _scheduledActions.Add(new ScheduledAction(nextExecutionTime, interval, action, true));
        }

        /// <summary>
        /// Schedules an action to be executed once after a delay.
        /// </summary>
        public void Once(double delayInSeconds, Action action)
        {
            var delay = TimeSpan.FromSeconds(delayInSeconds);
            var executionTime = DateTime.UtcNow + delay;
            _scheduledActions.Add(new ScheduledAction(executionTime, TimeSpan.Zero, action, false));
        }
    }
}
