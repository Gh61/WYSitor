using System.Windows.Input;

namespace Gh61.WYSitor.Interfaces
{
    internal interface IToolbarControl
    {
        void RegisterCommand(RoutedUICommand cmd);
    }
}
