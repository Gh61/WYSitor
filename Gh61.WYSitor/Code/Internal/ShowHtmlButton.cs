using System.Windows;
using System.Windows.Controls.Primitives;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;
using Gh61.WYSitor.ViewModels;

namespace Gh61.WYSitor.Code
{
    internal class ShowHtmlButton : ToolbarElement
    {
        public const string ElementIdentifier = "HtmlCode";

        private readonly ToolbarViewModel _toolbarViewModel;

        public ShowHtmlButton(ToolbarViewModel toolbarViewModel) : base(ElementIdentifier)
        {
            _toolbarViewModel = toolbarViewModel;
        }

        public override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            var button = new ToggleButton();
            button.ToolTip = Resources.Text_HtmlCode;
            button.Content = ResourceHelper.GetIcon("Icon_Code");
            button.Tag = ElementIdentifier;
            button.Name = ElementIdentifier;

            button.Click += (s, e) =>
            {
                // ReSharper disable once PossibleInvalidOperationException
                browserControl.ToggleSourceEditor(_toolbarViewModel, button.IsChecked.Value /*!browserControl.IsInSourceEditMode*/);
            };

            return button;
        }
    }
}
