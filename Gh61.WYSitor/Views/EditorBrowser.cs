using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Code.Interop;
using Gh61.WYSitor.Html;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.ViewModels;
using mshtml;

namespace Gh61.WYSitor.Views
{
    internal class EditorBrowser : UserControl, IBrowserControl
    {
        private bool _scriptErrorsHidden;
        private bool _firstDocumentOpened = false;
        private BrowserContextMenu _contextMenu;
        private WebBrowserEventsSubscriber _browserEvents;
        private readonly Throttler _htmlContentChangedEventThrottler = new Throttler(TimeConstants.MinHtmlChangedInterval);

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

        /// <summary>
        /// Fires when html content of this editor changes.
        /// </summary>
        internal event EventHandler HtmlContentChanged;

        private void FireHtmlContentChanged()
        {
            if (!CurrentDocument.IsCompletelyLoaded())
                return;

            _htmlContentChangedEventThrottler.Fire(() =>
            {
                HtmlContentChanged?.Invoke(this, new EventArgs());
            });
        }

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
            HtmlEditor.TextChanged += (s, e) => FireHtmlContentChanged();

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
            void FirstLoad(object sender, RoutedEventArgs args)
            {
                if (!_firstDocumentOpened)
                {
                    OpenDocument();
                }

                // link to browser events
                _browserEvents = WebBrowserEventsSubscriber.Create(Browser);
                _browserEvents.StatusTextChanged += (s, e) => FireHtmlContentChanged();

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
            TryHideScriptErrors(browser);
        }

        private SelectedRange _focusOutSelectedRange;
        private void TryFocusBody(bool useFocusOutRange = false)
        {
            if (!CurrentDocument.IsCompletelyLoaded())
                return;

            var selectedRange = GetSelectedRange();

            // when set, will search for last selected range
            if (useFocusOutRange && selectedRange.IsEmpty && _focusOutSelectedRange != null)
            {
                selectedRange = _focusOutSelectedRange;
            }

            // empty range = no range
            if (selectedRange.IsEmpty)
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

        private void TryHideScriptErrors(WebBrowser webBrowser)
        {
            if (_scriptErrorsHidden)
                return;

            _scriptErrorsHidden = InteropHelper.TryHideScriptErrors(webBrowser);
        }

        #endregion

        #region Public functions/interface implementation

        public void OpenDocument(string fileContent = null)
        {
            if(!_firstDocumentOpened)
                _firstDocumentOpened = true;

            var content = fileContent ?? Properties.Resources.Empty;

            // no need to change if it's the same content
            if (content == GetCurrentHtml())
                return;

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
                if (!CurrentDocument.IsCompletelyLoaded())
                    return string.Empty;

                var doc = CurrentDocument.documentElement.outerHTML;
                return doc.Replace(" contentEditable=true", "");
            }
        }

        public SelectedRange GetSelectedRange()
        {
            OnlyInBrowserMode();

            if (CurrentDocument.IsCompletelyLoaded())
            {
                var range = CurrentDocument.selection.createRange();
                if (range is IHTMLTxtRange typedRange)
                {
                    return new SelectedRange(typedRange);
                }
            }

            return SelectedRange.Empty;
        }

        public void ExecuteCommand(string commandId, bool showUI = false, object value = null)
        {
            OnlyInBrowserMode();

            if (!CurrentDocument.IsCompletelyLoaded())
                return;

            CurrentDocument.execCommand(commandId, showUI, value);
        }

        public object QueryCommandState(string commandId)
        {
            if (IsInSourceEditMode)
                return null;

            if (!CurrentDocument.IsCompletelyLoaded())
                return null;

            return CurrentDocument.queryCommandState(commandId);
        }

        public bool QueryCommandEnabled(string commandId)
        {
            if (IsInSourceEditMode)
                return false;

            if (!CurrentDocument.IsCompletelyLoaded())
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

            if (enableSourceEditor)
            {
                // insert latest html code to editor
                HtmlEditor.Text = GetCurrentHtml();

                // switch visibility
                HtmlEditor.Visibility = Visibility.Visible;
                Browser.Visibility = Visibility.Hidden;
            }
            else
            {
                // load edited html code back to browser
                Browser.NavigateToString(GetCurrentHtml());

                // switch visibility
                HtmlEditor.Visibility = Visibility.Hidden;
                Browser.Visibility = Visibility.Visible;
            }

            // switch toolbar
            model.SetSourceMode(enableSourceEditor);

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
