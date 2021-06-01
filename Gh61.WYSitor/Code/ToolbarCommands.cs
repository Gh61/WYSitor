using System;
using System.Windows.Documents;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;
using Gh61.WYSitor.ViewModels;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Class containing all standard toolbar operations.
    /// </summary>
    public static class ToolbarCommands
    {
        static ToolbarCommands()
        {
            Bold = new ToolbarButton("Bold", Resources.Text_Bold, new Bold(new Run("B")), ExecCommand("Bold"), CheckState("Bold"));
            Italic = new ToolbarButton("Italic", Resources.Text_Italic, new Italic(new Run("I")), ExecCommand("Italic"), CheckState("Italic"));
            Underline = new ToolbarButton("Underline", Resources.Text_Underline, new Underline(new Run("U")), ExecCommand("Underline"), CheckState("Underline"));

        }

        public static readonly ToolbarButton Bold;
        public static readonly ToolbarButton Italic;
        public static readonly ToolbarButton Underline;

        private static Action<IBrowserControl> ExecCommand(string command)
        {
            return (control => control.CurrentDocument.execCommand(command));
        }

        private static Func<IBrowserControl, bool> CheckState(string command)
        {
            return (control => control.CurrentDocument.queryCommandState(command));
        }

        internal static void RegisterAll(ToolbarViewModel model)
        {
            model.ToolbarElements.Add(Bold);
            model.ToolbarElements.Add(Italic);
            model.ToolbarElements.Add(Underline);
        }
    }
}
