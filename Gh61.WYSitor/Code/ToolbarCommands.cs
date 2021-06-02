using System;
using System.Windows;
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

            #region Buttons

            // Init resource for getting icons
            var icons = new ResourceDictionary();
            icons.Source = new Uri("/Gh61.WYSitor;component/Resources/Icons.xaml", UriKind.RelativeOrAbsolute);
            UIElement GetIcon(string name) => (UIElement) icons[name];

            Bold = new ToolbarButton("Bold", Resources.Text_Bold, GetIcon("Icon_Bold"), ExecCommand("Bold"), CheckState("Bold"));
            Italic = new ToolbarButton("Italic", Resources.Text_Italic, GetIcon("Icon_Italic"), ExecCommand("Italic"), CheckState("Italic"));
            Underline = new ToolbarButton("Underline", Resources.Text_Underline, GetIcon("Icon_Underline"), ExecCommand("Underline"), CheckState("Underline"));

            #endregion
        }

        public static readonly ToolbarElement FontFamily;
        public static readonly ToolbarElement FontSize;
        public static readonly ToolbarButton Bold;
        public static readonly ToolbarButton Italic;
        public static readonly ToolbarButton Underline;

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


        }
    }
}
