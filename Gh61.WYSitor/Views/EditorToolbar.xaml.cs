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
                Loaded += (s, e) =>
                {
                    HideToolbarOverflowToggleButton(CommandBar);
                    InitViewModel();
                };
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

        private static void HideToolbarOverflowToggleButton(ToolBar toolBar)
        {
            if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            if (toolBar.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        /// <summary>
        /// Gets current ViewModel.
        /// </summary>
        internal ToolbarViewModel ViewModel => DataContext as ToolbarViewModel;
    }
}
