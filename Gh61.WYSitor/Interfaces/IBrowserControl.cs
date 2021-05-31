using System.Windows.Controls;
using mshtml;

namespace Gh61.WYSitor.Interfaces
{
    public interface IBrowserControl
    {
        WebBrowser Browser { get; }

        HTMLDocument CurrentDocument { get; }
    }
}
