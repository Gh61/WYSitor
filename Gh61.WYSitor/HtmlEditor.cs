using System.Windows.Controls;
using Gh61.WYSitor.ViewModels;
using Gh61.WYSitor.Views;

namespace Gh61.WYSitor
{
    public class HtmlEditor : UserControl
    {
        public HtmlEditor()
        {
            Toolbar = new ToolbarViewModel();

            InitializeEditor();
        }

        private void InitializeEditor()
        {
            var dockPanel = new DockPanel();

            var browser = new EditorBrowser( /*TODO*/);
            var toolbar = new EditorToolbar(browser);

            DockPanel.SetDock(toolbar, Dock.Top);
            dockPanel.Children.Add(toolbar);
            DockPanel.SetDock(browser, Dock.Bottom);
            dockPanel.Children.Add(browser);

            Content = dockPanel;

            DataContext = this;
            toolbar.DataContext = Toolbar;
        }

        /// <summary>
        /// Gets toolbar view model.
        /// </summary>
        public ToolbarViewModel Toolbar
        {
            get;
        }
    }
}
