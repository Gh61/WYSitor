using System.Windows.Input;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.ViewModels
{
    public class ToolbarViewModel
    {
        /// <summary>
        /// Returns it the passed command can be now executed.
        /// </summary>
        internal bool CommandCanExecute(object sender, RoutedUICommand command, object parameter)
        {
            // all commands are allowed - for now

            return true;
        }

        internal void CommandExecuted(IBrowserControl browserCOntrol, RoutedUICommand command)
        {
            // TODO: add handling event
            var handled = false;

            if (!handled)
            {
                ToolbarCommands.HandleCommand(browserCOntrol, command);
            }
        }
    }
}
