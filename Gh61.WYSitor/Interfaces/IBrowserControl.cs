using System.Windows;
using System.Windows.Controls;
using mshtml;

namespace Gh61.WYSitor.Interfaces
{
    public interface IBrowserControl
    {
        /// <summary>
        /// Gets actual WebBrowser component.
        /// </summary>
        WebBrowser Browser { get; }

        /// <summary>
        /// Gets actually opened HTML document by <see cref="Browser"/>.
        /// </summary>
        HTMLDocument CurrentDocument { get; }

        bool IsLoaded { get; }
        event RoutedEventHandler Loaded;
        event RoutedEventHandler Unloaded;
    }
}
