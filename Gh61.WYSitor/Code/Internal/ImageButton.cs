using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
        }

        private static string CreateImageHtml(ImageDialog dialog)
        {
            if (TryGetBase64Data(dialog, out var pngBase64Data))
            {
                var hasTitle = !string.IsNullOrWhiteSpace(dialog.ImageTitle);
                // escaping quotes, so the attribute will not be broken by text with quotes
                var escapedTitle = (hasTitle ? dialog.ImageTitle : Resources.ImageForm_DefaultAltText).Replace("\"", "&quot;");

                var imgTag = new StringBuilder("<img ");
                imgTag.AppendFormat("src=\"data:image/png;base64,{0}\" ", pngBase64Data);
                imgTag.AppendFormat("alt=\"{0}\" ", hasTitle ? escapedTitle : Resources.ImageForm_DefaultAltText);
                if (hasTitle)
                {
                    imgTag.AppendFormat("title=\"{0}\" ", escapedTitle);
                }
                // specify size, so the page doesn't "jump" when loading
                imgTag.AppendFormat("width=\"{0}\" ", Math.Round(dialog.ImageWidth).ToString(NumberFormatInfo.InvariantInfo));
                imgTag.AppendFormat("height=\"{0}\" ", Math.Round(dialog.ImageHeight).ToString(NumberFormatInfo.InvariantInfo));
                imgTag.Append("/>");

                return imgTag.ToString();
            }
            else
            {
                // Inserting Error message
                return "[" + pngBase64Data + "]";
            }
        }

        private static bool TryGetBase64Data(ImageDialog dialog, out string pngBase64Data)
        {
            try
            {
                using (var image = Image.FromFile(dialog.ImagePath))
                {
                    Image smallerImage = null;
                    try
                    {
                        // physically reducing image size
                        if (dialog.ReduceCoefficient < 100 && dialog.DoPhysicalResize)
                        {
                            var w = (int)Math.Round(dialog.ImageWidth);
                            var h = (int)Math.Round(dialog.ImageHeight);

                            smallerImage = ResizeImage(image, w, h);
                        }

                        using (var stream = new MemoryStream())
                        {
                            // using smaller image, if it exists
                            if (smallerImage != null)
                            {
                                smallerImage.Save(stream, ImageFormat.Png);
                            }
                            else
                            {
                                image.Save(stream, ImageFormat.Png);
                            }

                            // read
                            byte[] data = new byte[stream.Length];
                            stream.Position = 0;
                            stream.Read(data, 0, data.Length);

                            pngBase64Data = Convert.ToBase64String(data);
                        }
                    }
                    finally
                    {
                        // maybe disposing smaller image
                        smallerImage?.Dispose();
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

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
