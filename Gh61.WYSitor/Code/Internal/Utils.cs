using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Gh61.WYSitor.Code
{
    internal static class Utils
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
    }
}
