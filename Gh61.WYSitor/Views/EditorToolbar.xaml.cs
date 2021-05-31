using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.ViewModels;

namespace Gh61.WYSitor.Views
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    internal partial class EditorToolbar : UserControl, IToolbarControl
    {
        public EditorToolbar()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.Loaded += ComponentLoaded;
            }
        }

        /// <summary>
        /// Called after the component is initialized.
        /// </summary>
        private void ComponentLoaded(object sender, RoutedEventArgs e)
        {
            ToolbarCommands.RegisterAll(this);
        }

        /// <summary>
        /// Gets current ViewModel.
        /// </summary>
        internal ToolbarViewModel ViewModel => DataContext as ToolbarViewModel;

        #region Commands

        public void RegisterCommand(RoutedUICommand cmd)
        {
            var cb = new CommandBinding(cmd, OnCommandExecuted, OnCommandCanExecute);
            CommandBindings.Add(cb);
        }

        private void OnCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!(e.Command is RoutedUICommand command))
                return;

            if (ViewModel == null)
            {
                e.CanExecute = false;
                return;
            }

            e.CanExecute = ViewModel.CommandCanExecute(sender, command, e.Parameter);
        }

        private void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ViewModel != null && e.Command is RoutedUICommand command)
            {
                ViewModel.CommandExecuted(sender, command, e.Parameter);
            }
        }

        #endregion
    }
}
