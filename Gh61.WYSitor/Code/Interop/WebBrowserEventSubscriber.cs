using System;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Gh61.WYSitor.Code.Interop
{
    /// <summary>
    /// Class for capturing internal web browser events.
    /// </summary>
    internal class WebBrowserEventsSubscriber : DWebBrowserEvents2
    {
        private readonly WebBrowser _browser;
        private readonly IConnectionPoint _icp;
        private readonly DispatcherTimer _fallbackTimer;
        private int _cookie = -1;

        #region ctor + dtor

        private WebBrowserEventsSubscriber(WebBrowser browser, object comWebBrowser)
        {
            _browser = browser;

            var container = (IConnectionPointContainer)comWebBrowser;

            var webBrowserEvents = typeof(DWebBrowserEvents2).GUID;
            container.FindConnectionPoint(ref webBrowserEvents, out _icp);
            _icp.Advise(this, out _cookie);
        }

        /// <summary>
        /// Ctor for fallback solution with timer.
        /// </summary>
        private WebBrowserEventsSubscriber(WebBrowser browser)
        {
            _browser = browser;

            _fallbackTimer = new DispatcherTimer();
            _fallbackTimer.Interval = TimeConstants.FallbackHtmlChangedInterval;
            _fallbackTimer.Tick += FallbackTimerTick;
            browser.Loaded += (s, e) => _fallbackTimer.Start();
            browser.Unloaded += (s, e) => _fallbackTimer.Stop();
            if (browser.IsLoaded)
            {
                _fallbackTimer.Start();
            }
        }

        private void FallbackTimerTick(object sender, EventArgs e)
        {
            if (_browser.IsVisible)
            {
                StatusTextChanged?.Invoke(_browser, new TextEventArgs(string.Empty));
            }
        }

        ~WebBrowserEventsSubscriber()
        {
            try
            {
                if (_cookie != -1)
                {
                    _icp.Unadvise(_cookie);
                }
            }
            catch (Exception)
            {
                // noop
            }
            _cookie = -1;
        }

        #endregion

        public static WebBrowserEventsSubscriber Create(WebBrowser browser, bool allowFallback = true)
        {
            if (!browser.IsLoaded)
                throw new InvalidOperationException("Subscriber can be created only if WebBrowser is loaded. (Call preferable on \"Loaded\" event.)");

            WebBrowserEventsSubscriber result;

            if (InteropHelper.TryGetInternalComWebBrowser(browser, out var comWebBrowser))
            {
                result = new WebBrowserEventsSubscriber(browser, comWebBrowser);
            }
            else if(allowFallback)
            {
                // internal comWebBrowser was not found - fallback to timer
                result = new WebBrowserEventsSubscriber(browser);
            }
            else
            {
                throw new InvalidOperationException("Internal web browser COM object not found");
            }

            return result;
        }

        #region Events

        public event EventHandler<TextEventArgs> StatusTextChanged;

        #endregion

        #region interface DWebBrowserEvents2

        void DWebBrowserEvents2.StatusTextChange(string text)
        {
            StatusTextChanged?.Invoke(_browser, new TextEventArgs(text));
        }

        void DWebBrowserEvents2.ProgressChange(int progress, int progressMax)
        {
        }

        void DWebBrowserEvents2.CommandStateChange(long command, bool enable)
        {
        }

        void DWebBrowserEvents2.DownloadBegin()
        {
        }

        void DWebBrowserEvents2.DownloadComplete()
        {
        }

        void DWebBrowserEvents2.TitleChange(string text)
        {
        }

        void DWebBrowserEvents2.PropertyChange(string szProperty)
        {
        }

        void DWebBrowserEvents2.PrintTemplateInstantiation(object pDisp)
        {
        }

        void DWebBrowserEvents2.PrintTemplateTeardown(object pDisp)
        {
        }

        void DWebBrowserEvents2.UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
        {
        }

        void DWebBrowserEvents2.BeforeNavigate2(object pDisp, ref object URL, ref object flags, ref object targetFrameName, ref object postData,
            ref object headers, ref bool cancel)
        {
        }

        void DWebBrowserEvents2.NewWindow2(ref object pDisp, ref bool cancel)
        {
        }

        void DWebBrowserEvents2.NavigateComplete2(object pDisp, ref object URL)
        {
        }

        void DWebBrowserEvents2.DocumentComplete(object pDisp, ref object URL)
        {
        }

        void DWebBrowserEvents2.OnQuit()
        {
        }

        void DWebBrowserEvents2.OnVisible(bool visible)
        {
        }

        void DWebBrowserEvents2.OnToolBar(bool toolBar)
        {
        }

        void DWebBrowserEvents2.OnMenuBar(bool menuBar)
        {
        }

        void DWebBrowserEvents2.OnStatusBar(bool statusBar)
        {
        }

        void DWebBrowserEvents2.OnFullScreen(bool fullScreen)
        {
        }

        void DWebBrowserEvents2.OnTheaterMode(bool theaterMode)
        {
        }

        void DWebBrowserEvents2.WindowSetResizable(bool resizable)
        {
        }

        void DWebBrowserEvents2.WindowSetLeft(int left)
        {
        }

        void DWebBrowserEvents2.WindowSetTop(int top)
        {
        }

        void DWebBrowserEvents2.WindowSetWidth(int width)
        {
        }

        void DWebBrowserEvents2.WindowSetHeight(int height)
        {
        }

        void DWebBrowserEvents2.WindowClosing(bool isChildWindow, ref bool cancel)
        {
        }

        void DWebBrowserEvents2.ClientToHostWindow(ref long cx, ref long cy)
        {
        }

        void DWebBrowserEvents2.SetSecureLockIcon(int secureLockIcon)
        {
        }

        void DWebBrowserEvents2.FileDownload(ref bool ActiveDocument, ref bool cancel)
        {
        }

        void DWebBrowserEvents2.NavigateError(object pDisp, ref object URL, ref object frame, ref object statusCode, ref bool cancel)
        {
        }

        void DWebBrowserEvents2.PrivacyImpactedStateChange(bool bImpacted)
        {
        }

        void DWebBrowserEvents2.SetPhishingFilterStatus(uint phishingFilterStatus)
        {
        }

        void DWebBrowserEvents2.WindowStateChanged(uint dwFlags, uint dwValidFlagsMask)
        {
        }

        #endregion
    }

    internal class TextEventArgs : EventArgs
    {
        public TextEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
