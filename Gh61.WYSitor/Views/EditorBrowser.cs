using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Html;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.ViewModels;
using mshtml;

namespace Gh61.WYSitor.Views
{
    internal class EditorBrowser : UserControl, IBrowserControl
    {
        private bool _scriptErrorsHidden;
        private BrowserContextMenu _contextMenu;

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
        internal WebBrowser Browser { get; set; }

        /// <summary>
        /// Gets html editor (typically hidden behind <see cref="Browser"/>).
        /// </summary>
        private TextBox HtmlEditor { get; set; }

        #region Browser init

        private void InitInternalBrowser()
        {
            if (this.Content != null)
                throw new InvalidOperationException("Browser is already initialized!");

            HtmlEditor = new TextBox();
            HtmlEditor.AcceptsReturn = true;
            HtmlEditor.AcceptsTab = true;
            HtmlEditor.AutoWordSelection = true;
            HtmlEditor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            HtmlEditor.TextWrapping = TextWrapping.Wrap;
            HtmlEditor.Visibility = Visibility.Hidden;

            Browser = new WebBrowser();
            Browser.LoadCompleted += DocumentLoaded;

            // custom menu
            _contextMenu = new BrowserContextMenu(this);

            // control for both - Html and Browser control
            var grid = new Grid();
            grid.Children.Add(HtmlEditor);
            grid.Children.Add(Browser);

            this.Content = grid;

            // Load default empty document
            void FirstLoad(object s, RoutedEventArgs e)
            {
                OpenDocument();
                Browser.Loaded -= FirstLoad;
            }
            Browser.Loaded += FirstLoad;
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
            var documentEvents = (HTMLDocumentEvents2_Event)CurrentDocument;
            documentEvents.onclick += (htmlEvent) =>
            {
                // click out of body tag will return focus back to body
                if (htmlEvent.srcElement?.GetType().Name == "HTMLHtmlElementClass")
                {
                    TryFocusBody();
                }

                return true;
            };
            // saving selected text, when control lost focus - so it can be back selected, for contextMenu
            documentEvents.onfocusout += (htmlEvent) =>
            {
                _focusOutSelectedRange = GetSelectedRange();
            };
            documentEvents.oncontextmenu += (htmlEvent) =>
            {
                // open custom menu
                _contextMenu.IsOpen = true;

                // focusing back selected text, if is
                TryFocusBody(true);

                // disable default context menu
                return false;
            };

            // document is loaded - internal browser should be loaded
            TryHideScriptErrors(browser, true);
        }

        private SelectedRange _focusOutSelectedRange;
        private void TryFocusBody(bool useFocusOutRange = false)
        {
            var selectedRange = GetSelectedRange();

            // when set, will search for last selected range
            if (useFocusOutRange && string.IsNullOrEmpty(selectedRange.Text))
            {
                selectedRange = _focusOutSelectedRange;
            }

            // empty range = no range
            if (string.IsNullOrEmpty(selectedRange.Text))
            {
                selectedRange = null;
            }

            ((HTMLBody)CurrentDocument.body)?.focus();

            // select back the selected text
            selectedRange?.Select();

            // clearing last selected range
            if(useFocusOutRange)
                _focusOutSelectedRange = null;
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

        #region Public functions/interface implementation

        public void OpenDocument(string fileContent = null)
        {
            var content = fileContent ?? Properties.Resources.Empty;

            if (IsInSourceEditMode)
            {
                HtmlEditor.Text = content;
            }
            else
            {
                Browser.NavigateToString(content);
            }
        }

        public string GetCurrentHtml()
        {
            if (IsInSourceEditMode)
            {
                return HtmlEditor.Text;
            }
            else
            {
                var doc = CurrentDocument.documentElement.innerHTML;
                return doc.Replace(" contentEditable=true", "");
            }
        }

        public SelectedRange GetSelectedRange()
        {
            OnlyInBrowserMode();

            return new SelectedRange((IHTMLTxtRange)CurrentDocument.selection.createRange());
        }

        public void ExecuteCommand(string commandId, bool showUI = false, object value = null)
        {
            OnlyInBrowserMode();

            if (CurrentDocument.readyState != "complete")
                return;

            CurrentDocument.execCommand(commandId, showUI, value);
        }

        public object QueryCommandState(string commandId)
        {
            if (IsInSourceEditMode)
                return null;

            return CurrentDocument.queryCommandState(commandId);
        }

        public bool QueryCommandEnabled(string commandId)
        {
            if (IsInSourceEditMode)
                return false;

            return CurrentDocument.queryCommandEnabled(commandId);
        }

        public void SetFont(FontFamily font)
        {
            ExecuteCommand("FontName", false, font.ToString());
        }

        public void SetFontSize(FontSize fontSize)
        {
            ExecuteCommand("FontSize", false, fontSize.Key);
        }

        void IBrowserControl.Focus()
        {
            if (IsInSourceEditMode)
            {
                HtmlEditor.Focus();
            }
            else
            {
                TryFocusBody();
            }
        }

        public bool IsInSourceEditMode => !Browser.IsVisible;

        public void ToggleSourceEditor(ToolbarViewModel model, bool enableSourceEditor)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // mode already set
            if (enableSourceEditor == IsInSourceEditMode)
                return;

            // cannot manage container Items when no container is present
            var itemsContainer = model.GetToolbarContainer();
            if (itemsContainer == null)
                return;

            var toolbarItems = itemsContainer.Items
                .OfType<FrameworkElement>()
                .Where(e => e.Tag?.ToString() != ShowHtmlButton.ElementIdentifier)
                .ToList();

            if (enableSourceEditor)
            {
                // insert latest html code to editor
                HtmlEditor.Text = GetCurrentHtml();

                // switch visibility
                HtmlEditor.Visibility = Visibility.Visible;
                Browser.Visibility = Visibility.Hidden;

                // disable all items
                toolbarItems.ForEach(i => i.IsEnabled = false);
            }
            else
            {
                // load edited html code back to browser
                Browser.NavigateToString(GetCurrentHtml());

                // switch visibility
                HtmlEditor.Visibility = Visibility.Hidden;
                Browser.Visibility = Visibility.Visible;

                // enable all items back
                toolbarItems.ForEach(i => i.IsEnabled = true);
            }

            ((IBrowserControl) this).Focus();
        }

        private void OnlyInBrowserMode()
        {
            if (IsInSourceEditMode)
                throw new InvalidOperationException("This operation can be used only in browser edit mode.");
        }

        #endregion
    }
}
