using System.Windows.Controls;
using Gh61.WYSitor.ViewModels;
using Gh61.WYSitor.Views;

namespace Gh61.WYSitor
{
    public class HtmlEditor : UserControl
    {
        public HtmlEditor()
        {
            InitializeEditor();
        }

        private void InitializeEditor()
        {
            var dockPanel = new DockPanel();

            var browser = new EditorBrowser( /*TODO*/);
            var toolbar = new EditorToolbar();

            DockPanel.SetDock(toolbar, Dock.Top);
            dockPanel.Children.Add(toolbar);
            DockPanel.SetDock(browser, Dock.Bottom);
            dockPanel.Children.Add(browser);

            Content = dockPanel;

            DataContext = this;
            toolbar.DataContext = Toolbar = new ToolbarViewModel(browser);
        }

        /// <summary>
        /// Gets toolbar view model.
        /// </summary>
        public ToolbarViewModel Toolbar
        {
            get;
            private set;
        }
    }
}
