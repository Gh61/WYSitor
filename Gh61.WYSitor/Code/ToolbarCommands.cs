using System;
using System.Linq;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Localization;
using Gh61.WYSitor.ViewModels;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Class containing all standard toolbar operations.
    /// </summary>
    public static class ToolbarCommands
    {
        /// <summary>
        /// Returns one of standard toolbar elements from passed ToolbarViewModel.
        /// Return null, if the element is not present.
        /// </summary>
        public static ToolbarElement Get(StandardToolbarElement elementType, ToolbarViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var identifier = elementType.ToString();
            var foundElement = model.ToolbarElements.FirstOrDefault(e => e.Identifier == identifier);
            return foundElement;
        }

        /// <summary>
        /// Creates new instance of standard toolbar element.
        /// Note, that you can't add another instance of the same control to the toolbar (because it has the same identifier).
        /// </summary>
        public static ToolbarElement Create(StandardToolbarElement elementType, ToolbarViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            switch (elementType)
            {
                case StandardToolbarElement.FontFamily:
                    return new FontPickerElement();
                case StandardToolbarElement.FontSize:
                    return new FontSizePicker();

                case StandardToolbarElement.Bold:
                    return new ToolbarButton(nameof(StandardToolbarElement.Bold), ResourceManager.Text_Bold, ResourceHelper.GetIcon("Icon_Bold"), ExecCommand("Bold"), CheckState("Bold"));
                case StandardToolbarElement.Italic:
                    return new ToolbarButton(nameof(StandardToolbarElement.Italic), ResourceManager.Text_Italic, ResourceHelper.GetIcon("Icon_Italic"), ExecCommand("Italic"), CheckState("Italic"));
                case StandardToolbarElement.Underline:
                    return new ToolbarButton(nameof(StandardToolbarElement.Underline), ResourceManager.Text_Underline, ResourceHelper.GetIcon("Icon_Underline"), ExecCommand("Underline"), CheckState("Underline"));

                case StandardToolbarElement.LineHighlightColor:
                    return new HighlightColorButton();
                case StandardToolbarElement.TextColor:
                    return new TextColorButton();

                case StandardToolbarElement.AlignLeft:
                    return new ToolbarButton(nameof(StandardToolbarElement.AlignLeft), ResourceManager.Text_AlignLeft, ResourceHelper.GetIcon("Icon_AlignLeft"), ExecCommand("JustifyLeft"), CheckState("JustifyLeft"));
                case StandardToolbarElement.AlignCenter:
                    return new ToolbarButton(nameof(StandardToolbarElement.AlignCenter), ResourceManager.Text_AlignCenter, ResourceHelper.GetIcon("Icon_AlignCenter"), ExecCommand("JustifyCenter"), CheckState("JustifyCenter"));
                case StandardToolbarElement.AlignRight:
                    return new ToolbarButton(nameof(StandardToolbarElement.AlignRight), ResourceManager.Text_AlignRight, ResourceHelper.GetIcon("Icon_AlignRight"), ExecCommand("JustifyRight"), CheckState("JustifyRight"));

                case StandardToolbarElement.BulletList:
                    return new ToolbarButton(nameof(StandardToolbarElement.BulletList), ResourceManager.Text_BulletList, ResourceHelper.GetIcon("Icon_BulletList"), ExecCommand("InsertUnorderedList"), CheckState("InsertUnorderedList"));
                case StandardToolbarElement.NumberedList:
                    return new ToolbarButton(nameof(StandardToolbarElement.NumberedList), ResourceManager.Text_NumberedList, ResourceHelper.GetIcon("Icon_NumberedList"), ExecCommand("InsertOrderedList"), CheckState("InsertOrderedList"));
                case StandardToolbarElement.Outdent:
                    return new ToolbarButton(nameof(StandardToolbarElement.Outdent), ResourceManager.Text_Outdent, ResourceHelper.GetIcon("Icon_Outdent"), ExecCommand("Outdent"));
                case StandardToolbarElement.Indent:
                    return new ToolbarButton(nameof(StandardToolbarElement.Indent), ResourceManager.Text_Indent, ResourceHelper.GetIcon("Icon_Indent"), ExecCommand("Indent"));

                case StandardToolbarElement.BackgroundColor:
                    return new BackgroundColorButton();
                case StandardToolbarElement.Image:
                    return new ImageButton();
                case StandardToolbarElement.Link:
                    return new LinkButton();

                case StandardToolbarElement.ToggleHtmlCode:
                    return new ShowHtmlButton(model);

                default:
                    throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
            }
        }

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
            model.ToolbarElements.Add(Create(StandardToolbarElement.FontFamily, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.FontSize, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.Bold, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.Italic, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.Underline, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.LineHighlightColor, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.TextColor, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.AlignLeft, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.AlignCenter, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.AlignRight, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.BulletList, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.NumberedList, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.Outdent, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.Indent, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.BackgroundColor, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.Image, model));
            model.ToolbarElements.Add(Create(StandardToolbarElement.Link, model));
            model.ToolbarElements.Add(new ToolbarSeparatorElement());

            model.ToolbarElements.Add(Create(StandardToolbarElement.ToggleHtmlCode, model));
        }
    }
}
