using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                
            }

            // TODO: AddImage
            // TODO: Disable browser context menu (on right click)
            // TODO: Binding or other way how to get html content (Binding preferably)
        }

        private static string CreateImageHtml(ImageDialog dialog)
        {
            throw new NotImplementedException();
        }

        private static bool TryGetBase64Data(string imagePath, out string base64Data)
        {
            base64Data = null;

            try
            {
                using (var image = Image.FromFile(imagePath))
                {
                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, ImageFormat.Png);
                        base64Data = Convert.ToBase64String(stream.GetBuffer());
                    }
                }

                return true;
            }
            catch
            {
                // noop
                return false;
            }
        }
    }
}
