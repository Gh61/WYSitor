using System;
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
            FontFamily = new FontPickerElement();
            FontSize = new FontSizePicker();

            Bold = new ToolbarButton("Bold", Resources.Text_Bold, ResourceHelper.GetIcon("Icon_Bold"), ExecCommand("Bold"), CheckState("Bold"));
            Italic = new ToolbarButton("Italic", Resources.Text_Italic, ResourceHelper.GetIcon("Icon_Italic"), ExecCommand("Italic"), CheckState("Italic"));
            Underline = new ToolbarButton("Underline", Resources.Text_Underline, ResourceHelper.GetIcon("Icon_Underline"), ExecCommand("Underline"), CheckState("Underline"));
            LineHighlightColor = new HighlightColorButton();
        }

        public static readonly ToolbarElement FontFamily;
        public static readonly ToolbarElement FontSize;
        public static readonly ToolbarButton Bold;
        public static readonly ToolbarButton Italic;
        public static readonly ToolbarButton Underline;
        public static readonly ToolbarSplitButton LineHighlightColor;

        /// <summary>
        /// Restores toolbar elements to default.
        /// </summary>
        /// <param name="model">HtmlEditor.Toolbar</param>
        public static void RestoreAll(ToolbarViewModel model)
        {
            model.ToolbarElements.Clear();

            RegisterAll(model);
        }

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
            model.ToolbarElements.Add(FontFamily);
            model.ToolbarElements.Add(FontSize);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Bold);
            model.ToolbarElements.Add(Italic);
            model.ToolbarElements.Add(Underline);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            LineHighlightColor.Register(model);
        }
    }
}
