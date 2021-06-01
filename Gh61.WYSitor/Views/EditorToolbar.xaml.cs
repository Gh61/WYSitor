using System.Windows;
using System.Windows.Controls;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.ViewModels;

namespace Gh61.WYSitor.Views
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    internal partial class EditorToolbar : UserControl
    {
        public EditorToolbar()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Loaded += (s, e) => InitViewModel();
                DataContextChanged += (s, e) => InitViewModel();
            }
        }

        private bool _isInitialized;
        private void InitViewModel()
        {
            if (!_isInitialized && ViewModel != null)
            {
                ViewModel.SetToolbarContainer(CommandBar);
                ToolbarCommands.RegisterAll(ViewModel);
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Gets current ViewModel.
        /// </summary>
        internal ToolbarViewModel ViewModel => DataContext as ToolbarViewModel;
    }
}
