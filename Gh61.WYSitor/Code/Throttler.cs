using System;
using System.Windows.Threading;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// This class adds the possibility to throttle repeated function calling.
    /// When the <see cref="Fire"/> function is called repeatedly, will only actually call the original function at most once per every interval.
    /// Useful for rate-limiting events that occur faster than you can keep up with.
    /// </summary>
    /// <remarks>
    /// Will execute the function as soon as you call it for the first time, and, if you call it again any number of times during the wait period, as soon as that period is over.
    /// </remarks>
    internal class Throttler
    {
        private readonly DispatcherTimer _throttleTimer;
        private readonly object _actionLock = new object();
        private Action _queuedAction;

        public Throttler(TimeSpan interval)
        {
            _throttleTimer = new DispatcherTimer();
            _throttleTimer.Interval = interval;
            _throttleTimer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            // stop the timer immediately
            _throttleTimer.Stop();

            // invoke and clear the action
            lock (_actionLock)
            {
                if (_queuedAction != null)
                {
                    _queuedAction();
                    _queuedAction = null;
                }
            }
        }

        public void Fire(Action action)
        {
            // the timer is not running, invoking action immediately and starting the timer
            if (!_throttleTimer.IsEnabled)
            {
                action();
                _throttleTimer.Start();
            }
            // timer is already running - queueing the action
            else
            {
                lock (_actionLock)
                {
                    // is the timer still running?
                    if (_throttleTimer.IsEnabled)
                    {
                        // if so, queue the action
                        _queuedAction = action;
                    }
                    else
                    {
                        // if the timer is off - invoke the action and start the timer
                        action();
                        _throttleTimer.Start();
                    }
                }
            }
        }
    }
}
