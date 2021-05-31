using System.Windows.Input;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Class containing all standard toolbar operations.
    /// </summary>
    public static class ToolbarCommands
    {
        public const string BoldName = "Bold";
        public const string ItalicName = "Italic";
        public const string UnderlineName = "Underline";

        public static readonly RoutedUICommand Bold = new RoutedUICommand("Bold", BoldName, typeof(ToolbarCommands));
        public static readonly RoutedUICommand Italic = new RoutedUICommand("Italic", ItalicName, typeof(ToolbarCommands));
        public static readonly RoutedUICommand Underline = new RoutedUICommand("Underline", UnderlineName, typeof(ToolbarCommands));

        internal static void RegisterAll(IToolbarControl toolbar)
        {
            toolbar.RegisterCommand(ToolbarCommands.Bold);
            toolbar.RegisterCommand(ToolbarCommands.Italic);
            toolbar.RegisterCommand(ToolbarCommands.Underline);
        }

        internal static void HandleCommand(IBrowserControl control, RoutedUICommand command)
        {
            switch (command.Name)
            {
                case BoldName:
                    control.CurrentDocument.execCommand("Bold");
                    break;
                case ItalicName:
                    control.CurrentDocument.execCommand("Italic");
                    break;
                case UnderlineName:
                    control.CurrentDocument.execCommand("Underline");
                    break;
            }
        }
    }
}
