using System.Windows.Controls;
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

            var toolbar = new EditorToolbar();
            DockPanel.SetDock(toolbar, Dock.Top);
            dockPanel.Children.Add(toolbar);

            var browser = new EditorBrowser(/*TODO*/);
            DockPanel.SetDock(browser, Dock.Bottom);
            dockPanel.Children.Add(browser);

            this.Content = dockPanel;
        }
    }
}
