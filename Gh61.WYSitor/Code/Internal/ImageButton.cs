using System.Windows;
using Gh61.WYSitor.Dialogs;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;
using Gh61.WYSitor.Views;

namespace Gh61.WYSitor.Code
{
    internal class ImageButton : ToolbarButton
    {
        public ImageButton() : base("Image", Resources.Text_Image, ResourceHelper.GetIcon("Icon_Image"), null)
        {
        }

        public override void Clicked(IBrowserControl browserControl)
        {
            var imageDialog = new ImageDialog();
            imageDialog.Owner = Window.GetWindow((EditorBrowser)browserControl);
            if (imageDialog.ShowDialog() == true)
            {
                //TODO
            }

            // TODO: AddImage
            // TODO: Disable browser context menu (on right click)
            // TODO: Binding or other way how to get html content (Binding preferably)
        }
    }
}
