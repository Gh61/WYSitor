using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.ViewModels
{
    public class ToolbarViewModel
    {
        private readonly IBrowserControl _browser;
        private DispatcherTimer _styleCheckTimer;

        public ToolbarViewModel(IBrowserControl browser)
        {
            _browser = browser;

            InitStyleCheckTimer();
        }

        /// <summary>
        /// Returns it the passed command can be now executed.
        /// </summary>
        internal bool CommandCanExecute(object sender, RoutedUICommand command, object parameter)
        {
            // all commands are allowed - for now

            return true;
        }

        internal void CommandExecuted(object sender, RoutedUICommand command, object parameter)
        {
            // TODO: add handling event
            var handled = false;

            if (!handled)
            {
                ToolbarCommands.HandleCommand(_browser, command);
            }
        }

        #region Style-check timer

        private void InitStyleCheckTimer()
        {
            _styleCheckTimer = new DispatcherTimer();
            _styleCheckTimer.Interval = TimeSpan.FromMilliseconds(2000); // TODO: back to 200ms
            _styleCheckTimer.Tick += TryCheckStyle;

            // Timer running only when browser is loaded
            _browser.Unloaded += (s, e) => _styleCheckTimer.Stop();
            _browser.Loaded += (s, e) => _styleCheckTimer.Start();
            if (_browser.IsLoaded)
            {
                _styleCheckTimer.Start();
            }
        }

        private void TryCheckStyle(object sender, EventArgs e)
        {
            if (_browser.CurrentDocument.readyState != "complete")
                return;

            Debug.WriteLine("Is Bold: " + _browser.CurrentDocument.queryCommandState("Bold"));
        }

        #endregion
    }
}
