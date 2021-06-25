using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;

namespace Gh61.WYSitor.Code
{
    internal class BackgroundColorButton : ToolbarElement
    {
        public BackgroundColorButton() : base(nameof(StandardToolbarElement.BackgroundColor))
        {
        }

        protected override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            /*
             * <ToggleButton>
                   <ToggleButton.ContextMenu>...</ToggleButton.ContextMenu>
                   <Viewbox Height="16">
                       <Grid>
                           <ContentControl Content="{StaticResource Icon_Bucket}"/>
                           <ContentControl Content="{StaticResource Icon_Bucket_UnderscoreAndDrop}"/>
                       </Grid>
                   </Viewbox>
               </ToggleButton>
             */

            var button = new ToggleButton();
            button.ToolTip = Resources.Text_BackgroundColor;

            var scaler = new Viewbox();
            scaler.Height = 16;

            var container = new Grid();
            var icon = ResourceHelper.GetIcon("Icon_Bucket_Main");
            var underscore = (Path)ResourceHelper.GetIcon("Icon_Bucket_UnderscoreAndDrop");
            container.Children.Add(icon);
            container.Children.Add(underscore);

            scaler.Child = container;
            button.Content = scaler;

            var colorPicker = new ColorPickerControl(Colors.White);
            underscore.Fill = colorPicker.LastSelectedBrush;

            var contextMenu = colorPicker.CreateContextMenu();
            Utils.SetContextMenuAndPosition(contextMenu, button);
            Utils.SetContextMenuOpening(contextMenu, button);

            colorPicker.ColorSelected += (s, e) =>
            {
                underscore.Fill = e.SelectedBrush;
                browserControl.CurrentDocument.body.style.background = Utils.FormatColor(e.SelectedColor);
                browserControl.Focus();
            };

            return button;

        }
    }
}
