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
                    if (!InitViewModel())
                    {
                        AdjustToolbarOverflowToggleButton(CommandBar);
                    }
                };
                DataContextChanged += (s, e) => InitViewModel();
            }
        }

        private bool _isInitialized;
        private bool InitViewModel()
        {
            if (!_isInitialized && ViewModel != null)
            {
                ViewModel.SetToolbarContainer(CommandBar);
                ToolbarCommands.RegisterAll(ViewModel);
                ViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ViewModel.EnableOverflowMode))
                    {
                        AdjustToolbarOverflowToggleButton(CommandBar);
                    }
                };

                AdjustToolbarOverflowToggleButton(CommandBar);
                _isInitialized = true;

                return true;
            }

            return false;
        }

        #region Overflow button

        private Visibility? _overflowGridDefaultVisibility;
        private Thickness? _mainPanelBorderDefaultMargin;

        private void AdjustToolbarOverflowToggleButton(ToolBar toolBar)
        {
            if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
            {
                if (_overflowGridDefaultVisibility == null)
                {
                    _overflowGridDefaultVisibility = overflowGrid.Visibility;
                }

                overflowGrid.Visibility = ViewModel.EnableOverflowMode
                    ? _overflowGridDefaultVisibility.Value
                    : Visibility.Collapsed;
            }

            if (toolBar.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
            {
                if (_mainPanelBorderDefaultMargin == null)
                {
                    _mainPanelBorderDefaultMargin = mainPanelBorder.Margin;
                }

                mainPanelBorder.Margin = ViewModel.EnableOverflowMode
                    ? _mainPanelBorderDefaultMargin.Value
                    : new Thickness();
            }
        }

        #endregion

        /// <summary>
        /// Gets current ViewModel.
        /// </summary>
        internal ToolbarViewModel ViewModel => DataContext as ToolbarViewModel;
    }
}
