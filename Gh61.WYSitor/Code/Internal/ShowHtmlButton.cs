using System.Windows;
using System.Windows.Controls.Primitives;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;

namespace Gh61.WYSitor.Code
{
    internal class ShowHtmlButton : ToolbarElement
    {
        public ShowHtmlButton() : base("HtmlCode")
        {
        }

        public override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            var button = new ToggleButton();
            button.ToolTip = Resources.Text_HtmlCode;
            button.Content = ResourceHelper.GetIcon("Icon_Code");

            button.Click += OnClick;

            return button;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TODO");
        }

        // TODO: ShowHtml
        // TODO: AddImage
        // TODO: AddLink
        // TODO: Prepare Image for insertion of []
        // TODO: Disable browser context menu (on right click)
        // TODO: Binding or other way how to get html content (Binding preferably)
    }
}
