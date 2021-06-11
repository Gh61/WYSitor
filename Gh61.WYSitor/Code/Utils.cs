using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using mshtml;

namespace Gh61.WYSitor.Code
{
    public static class Utils
    {
        /// <summary>
        /// Will set positioning of opening context menu under the passed target.
        /// </summary>
        public static void SetContextMenuAndPosition(ContextMenu contextMenu, FrameworkElement target)
        {
            contextMenu.PlacementTarget = target;
            contextMenu.HasDropShadow = false;
            contextMenu.Placement = PlacementMode.Left;
            contextMenu.Opened += (s, e) =>
            {
                contextMenu.HorizontalOffset = -target.ActualWidth;
                contextMenu.VerticalOffset = target.ActualHeight;
            };

            target.ContextMenu = contextMenu;
        }

        /// <summary>
        /// Will set context menu opening on click of this passed button.
        /// </summary>
        public static void SetContextMenuOpening(ContextMenu contextMenu, ToggleButton button)
        {
            button.Click += (s, e) =>
            {
                // ReSharper disable once PossibleInvalidOperationException
               contextMenu.IsOpen = button.IsChecked.Value;
            };
            contextMenu.Closed += (s, e) => { button.IsChecked = false; };
        }

        /// <summary>
        /// Format color, so it can be used for command in browser.
        /// </summary>
        public static string FormatColor(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        #region Hide Icon

        internal const int SWP_NOSIZE = 0x0001;
        internal const int SWP_NOMOVE = 0x0002;
        internal const int SWP_NOZORDER = 0x0004;
        internal const int SWP_FRAMECHANGED = 0x0020;
        internal const int GWL_EXSTYLE = -20;
        internal const int WS_EX_DLGMODALFRAME = 0x0001;

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        /// <summary>
        /// Hides icon for window.
        /// If this is called before InitializeComponent() then the icon will be completely removed from the title bar
        /// If this is called after InitializeComponent() then an empty image is used but there will be empty space between window border and title
        /// </summary>
        /// <param name="window">Window class</param>
        internal static void HideIcon(this Window window)
        {
            if (window.IsInitialized)
            {
                window.Icon = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgra32, null, new byte[] {0, 0, 0, 0}, 4);
            }
            else
            {
                window.SourceInitialized += delegate
                {
                    // Get this window's handle
                    var hwnd = new WindowInteropHelper(window).Handle;

                    // Change the extended window style to not show a window icon
                    int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                    SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

                    // Update the window's non-client area to reflect the changes
                    SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
                };
            }
        }

        #endregion

        /// <summary>
        /// Will safely select all text in passed textBox.
        /// </summary>
        internal static void SelectAllText(this TextBox textBox)
        {
            textBox.Dispatcher.BeginInvoke(
                new Action(textBox.SelectAll),
                DispatcherPriority.Input
            );
        }

        /// <summary>
        /// Will escape special HTML characters to html entities.
        /// </summary>
        public static string EscapeHtml(this string value, bool simple = false)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("<", "&lt;");
                if (!simple)
                {
                    value = value.Replace(">", "&gt;");
                    value = value.Replace(" ", "&nbsp;");
                    value = value.Replace("\"", "&quot;");
                    value = value.Replace("\'", "&#39;");
                    value = value.Replace("&", "&amp;");
                }
                return value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns whether the current document is completely loaded.
        /// </summary>
        public static bool IsCompletelyLoaded(this HTMLDocument document)
        {
            if (document == null)
                return false;

            if (document.readyState != "complete")
                return false;

            return true;
        }
    }
}
