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

            Bold = new ToolbarButton(nameof(Bold), Resources.Text_Bold, ResourceHelper.GetIcon("Icon_Bold"), ExecCommand("Bold"), CheckState("Bold"));
            Italic = new ToolbarButton(nameof(Italic), Resources.Text_Italic, ResourceHelper.GetIcon("Icon_Italic"), ExecCommand("Italic"), CheckState("Italic"));
            Underline = new ToolbarButton(nameof(Underline), Resources.Text_Underline, ResourceHelper.GetIcon("Icon_Underline"), ExecCommand("Underline"), CheckState("Underline"));

            LineHighlightColor = new HighlightColorButton();
            TextColor = new TextColorButton();

            AlignLeft = new ToolbarButton(nameof(AlignLeft), Resources.Text_Bold, ResourceHelper.GetIcon("Icon_AlignLeft"), ExecCommand("JustifyLeft"), CheckState("JustifyLeft"));
            AlignCenter = new ToolbarButton(nameof(AlignCenter), Resources.Text_Bold, ResourceHelper.GetIcon("Icon_AlignCenter"), ExecCommand("JustifyCenter"), CheckState("JustifyCenter"));
            AlignRight = new ToolbarButton(nameof(AlignRight), Resources.Text_Bold, ResourceHelper.GetIcon("Icon_AlignRight"), ExecCommand("JustifyRight"), CheckState("JustifyRight"));

            BulletList = new ToolbarButton(nameof(BulletList), Resources.Text_BulletList, ResourceHelper.GetIcon("Icon_BulletList"), ExecCommand("InsertUnorderedList"), CheckState("InsertUnorderedList"));
            NumberedList = new ToolbarButton(nameof(NumberedList), Resources.Text_NumberedList, ResourceHelper.GetIcon("Icon_NumberedList"), ExecCommand("InsertOrderedList"), CheckState("InsertOrderedList"));
            Outdent = new ToolbarButton(nameof(Outdent), Resources.Text_Outdent, ResourceHelper.GetIcon("Icon_Outdent"), ExecCommand("Outdent"));
            Indent = new ToolbarButton(nameof(Indent), Resources.Text_Indent, ResourceHelper.GetIcon("Icon_Indent"), ExecCommand("Indent"));

            BackgroundColor = new BackgroundColorButton();
            Image = new ImageButton();
            Link = new LinkButton();

            ShowHtml = new ShowHtmlButton();
        }

        public static readonly ToolbarElement FontFamily;
        public static readonly ToolbarElement FontSize;
        public static readonly ToolbarButton Bold;
        public static readonly ToolbarButton Italic;
        public static readonly ToolbarButton Underline;
        public static readonly ToolbarSplitButton LineHighlightColor;
        public static readonly ToolbarSplitButton TextColor;
        public static readonly ToolbarButton AlignLeft;
        public static readonly ToolbarButton AlignCenter;
        public static readonly ToolbarButton AlignRight;
        public static readonly ToolbarButton BulletList;
        public static readonly ToolbarButton NumberedList;
        public static readonly ToolbarButton Outdent;
        public static readonly ToolbarButton Indent;
        public static readonly ToolbarElement BackgroundColor;
        public static readonly ToolbarButton Image;
        public static readonly ToolbarButton Link;
        public static readonly ToolbarElement ShowHtml;

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
            TextColor.Register(model);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(AlignLeft);
            model.ToolbarElements.Add(AlignCenter);
            model.ToolbarElements.Add(AlignRight);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(BulletList);
            model.ToolbarElements.Add(NumberedList);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Outdent);
            model.ToolbarElements.Add(Indent);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(BackgroundColor);
            model.ToolbarElements.Add(Image);
            model.ToolbarElements.Add(Link);
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(ShowHtml);
        }
    }
}
