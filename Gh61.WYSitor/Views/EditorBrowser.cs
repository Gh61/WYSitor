using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Navigation;
using Gh61.WYSitor.Interfaces;
using mshtml;

namespace Gh61.WYSitor.Views
{
    internal class EditorBrowser : UserControl, IBrowserControl
    {
        private readonly string _startDocument;
        private bool _scriptErrorsHidden;

        public EditorBrowser(string startDocument = null)
        {
            _startDocument = startDocument;

            InitInternalBrowser();
        }

        /// <summary>
        /// Gets actually opened HTML document by <see cref="Browser"/>.
        /// </summary>
        public HTMLDocument CurrentDocument { get; private set; }

        /// <summary>
        /// Gets 
        /// </summary>
        public WebBrowser Browser => Content as WebBrowser;

        private void InitInternalBrowser()
        {
            if (this.Content != null)
                throw new InvalidOperationException("Browser is already initialized!");

            var browser = new WebBrowser();
            browser.LoadCompleted += DocumentLoaded;
            this.Content = browser;

            browser.NavigateToString(_startDocument ?? Properties.Resources.Empty);
        }

        private void DocumentLoaded(object sender, NavigationEventArgs e)
        {
            var browser = (WebBrowser)sender;

            if (!(browser.Document is HTMLDocument document))
                throw new NotSupportedException("WebBrowser.Document is not instance of type HTMLDocument.");

            document.designMode = "On";
            CurrentDocument = document;

            // document is loaded - internal browser should be loaded
            TryHideScriptErrors(browser, true);
        }

        private void TryHideScriptErrors(WebBrowser wb, bool hide)
        {
            if (_scriptErrorsHidden)
                return;

            // trying to get private field of COM component of browser
            var comWebBrowserField = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (comWebBrowserField == null)
            {
                // was not found - ending
                return;
            }

            // getting private COM component of browser
            object comWebBrowser = comWebBrowserField.GetValue(wb);
            if (comWebBrowser == null)
            {
                // is null, ending
                return;
            }

            // setting script errors to silent
            comWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object[] { hide });
            _scriptErrorsHidden = true;
        }
    }
}
