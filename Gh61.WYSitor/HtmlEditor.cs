using System;
using System.Diagnostics;
using System.Windows;
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

            var browser = new EditorBrowser();
            var toolbar = new EditorToolbar();

            DockPanel.SetDock(toolbar, Dock.Top);
            dockPanel.Children.Add(toolbar);
            DockPanel.SetDock(browser, Dock.Bottom);
            dockPanel.Children.Add(browser);

            // dependency on html content
            browser.HtmlContentChanged += BrowserContentChanged;

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

        #region HtmlContent dependency property

        /// <summary>
        /// Gets or sets html content of this editor.
        /// </summary>
        public string HtmlContent
        {
            get => (string)GetValue(HtmlContentProperty);
            set => SetValue(HtmlContentProperty, value);
        }
        public static readonly DependencyProperty HtmlContentProperty = DependencyProperty.Register(nameof(HtmlContent), typeof(string), typeof(HtmlEditor), new PropertyMetadata(HtmlContentPropertyChanged));

        private static void HtmlContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (HtmlEditor)d;

            Debug.WriteLine("HtmlContentPropChanged: " + e.NewValue + "\r\n----------------------------------");

            editor.Browser.OpenDocument((string)e.NewValue);
        }

        private void BrowserContentChanged(object sender, EventArgs e)
        {
            var content = Browser.GetCurrentHtml();

            Debug.WriteLine("BrowserContentChanged: " + content + "\r\n----------------------------------");

            HtmlContent = content;
        }

        #endregion
    }
}
