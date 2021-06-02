using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Gh61.WYSitor.Html;
using Gh61.WYSitor.Interfaces;
using mshtml;

namespace Gh61.WYSitor.Views
{
    internal class EditorBrowser : UserControl, IBrowserControl
    {
        private bool _scriptErrorsHidden;

        public EditorBrowser()
        {
            InitInternalBrowser();
        }

        /// <summary>
        /// Gets actually opened HTML document by <see cref="Browser"/>.
        /// </summary>
        public HTMLDocument CurrentDocument { get; private set; }

        /// <summary>
        /// Gets actual WebBrowser component.
        /// </summary>
        private WebBrowser Browser => Content as WebBrowser;

        #region Browser init

        private void InitInternalBrowser()
        {
            if (this.Content != null)
                throw new InvalidOperationException("Browser is already initialized!");

            var browser = new WebBrowser();
            browser.LoadCompleted += DocumentLoaded;
            this.Content = browser;

            // Load default empty document
            void FirstLoad(object s, RoutedEventArgs e)
            {
                OpenDocument();
                browser.Loaded -= FirstLoad;
            }
            browser.Loaded += FirstLoad;
        }

        private void DocumentLoaded(object sender, NavigationEventArgs e)
        {
            var browser = (WebBrowser)sender;

            if (!(browser.Document is HTMLDocument document))
                throw new NotSupportedException("WebBrowser.Document is not instance of type HTMLDocument.");

            CurrentDocument = document;

            //CurrentDocument.designMode = "on"; // will clear the entire document on x64 systems

            // create body, so i can turn on editable mode
            if (CurrentDocument.body == null)
            {
                throw new InvalidOperationException("Passed html element contains no body");
            }

            ((HTMLBody)CurrentDocument.body).contentEditable = "true";
            ((HTMLDocumentEvents2_Event)CurrentDocument).onclick += (htmlEvent) =>
            {
                // click out of body tag will return focus back to body
                if (htmlEvent.srcElement is HTMLHtmlElement)
                {
                    TryFocusBody();
                }

                return true;
            };
            TryFocusBody();

            // document is loaded - internal browser should be loaded
            TryHideScriptErrors(browser, true);
        }

        private void TryFocusBody()
        {
            ((HTMLBody)CurrentDocument.body)?.focus();
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

        #endregion

        #region Public functions

        public void OpenDocument(string fileContent = null)
        {
            Browser.NavigateToString(fileContent ?? Properties.Resources.Empty);
        }

        public void ExecuteCommand(string commandId, bool showUI = false, object value = null)
        {
            if (CurrentDocument.readyState != "complete")
                return;

            CurrentDocument.execCommand(commandId, showUI, value);
        }

        public void SetFont(FontFamily font)
        {
            ExecuteCommand("FontName", false, font.ToString());
        }

        public void SetFontSize(FontSize fontSize)
        {
            ExecuteCommand("FontSize", false, fontSize.Key);
        }

        public void Focus()
        {
            TryFocusBody();
        }

        #endregion
    }
}
