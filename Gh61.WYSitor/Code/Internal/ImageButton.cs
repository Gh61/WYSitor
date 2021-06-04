using System.Windows;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;

namespace Gh61.WYSitor.Code
{
    internal class ImageButton : ToolbarButton
    {
        public ImageButton() : base("Image", Resources.Text_Image, ResourceHelper.GetIcon("Icon_Image"), null)
        {
        }

        public override void Clicked(IBrowserControl browserControl)
        {
            MessageBox.Show("TODO: Open dialog box");
        }
    }
}
