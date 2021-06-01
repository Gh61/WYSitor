using System.Windows.Controls;
using Gh61.WYSitor.Interfaces;
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

            // set DataContexts and properties
            DataContext = this;
            toolbar.DataContext = Toolbar = new ToolbarViewModel(browser);
            Browser = browser;

            // render new content
            Content = dockPanel;
        }

        /// <summary>
        /// Gets toolbar view model.
        /// </summary>
        public ToolbarViewModel Toolbar
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets browser control.
        /// </summary>
        public IBrowserControl Browser
        {
            get;
            private set;
        }
    }
}
