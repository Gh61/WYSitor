using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
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
            if (imageDialog.ShowDialog() == true && imageDialog.IsSuccess)
            {
                var imgTag = CreateImageHtml(imageDialog);
                browserControl.GetSelectedRange().PasteHtml(imgTag);
            }

            // TODO: Disable browser context menu (on right click)
            // TODO: Binding or other way how to get html content (Binding preferably)
        }

        private static string CreateImageHtml(ImageDialog dialog)
        {
            if (TryGetBase64Data(dialog.ImagePath, out var pngBase64Data))
            {
                var hasTitle = !string.IsNullOrWhiteSpace(dialog.Title);
                // escaping quotes, so the attribute will not be broken by text with quotes
                var escapedTitle = (hasTitle ? dialog.Title : Resources.ImageForm_DefaultAltText).Replace("\"", "&quot;");

                var imgTag = new StringBuilder("<img ");
                imgTag.AppendFormat("src=\"data:image/png;base64,{0}\" ", pngBase64Data);
                imgTag.AppendFormat("alt=\"{0}\" ", hasTitle ? escapedTitle : Resources.ImageForm_DefaultAltText);
                if (hasTitle)
                {
                    imgTag.AppendFormat("title=\"{0}\" ", escapedTitle);
                }
                // specify size, so the page doesn't "jump" when loading
                imgTag.AppendFormat("width=\"{0}\" ", dialog.ImageWidth.ToString(NumberFormatInfo.InvariantInfo));
                imgTag.AppendFormat("height=\"{0}\" ", dialog.ImageHeight.ToString(NumberFormatInfo.InvariantInfo));
                imgTag.Append("/>");

                return imgTag.ToString();
            }
            else
            {
                // Inserting Error message
                return "[" + pngBase64Data + "]";
            }
        }

        private static bool TryGetBase64Data(string imagePath, out string pngBase64Data)
        {
            try
            {
                using (var image = Image.FromFile(imagePath))
                {
                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, ImageFormat.Png);
                        pngBase64Data = Convert.ToBase64String(stream.GetBuffer());
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                // noop
                pngBase64Data = e.Message;
                return false;
            }
        }
    }
}
