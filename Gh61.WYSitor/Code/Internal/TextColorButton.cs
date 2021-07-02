using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Localization;

namespace Gh61.WYSitor.Code
{
    internal class TextColorButton : ColorButtonBase
    {
        public TextColorButton() : base(nameof(StandardToolbarElement.TextColor), ResourceManager.Text_TextColor, Colors.Red)
        {
        }

        protected override UIElement CreateMainButtonContent()
        {
            /*
             * <Viewbox Height="16">
                    <Grid>
                        <ContentControl Content="{StaticResource Icon_TextColor_Main}" Margin="-2,0,2,0" />
                        <ContentControl Content="{StaticResource Icon_TextColor_Underscore}" Margin="-2,0,2,0" />
                    </Grid>
                </Viewbox>
             */

            var scaler = new Viewbox();
            scaler.Height = 16;

            var container = new Grid();
            var icon = ResourceHelper.GetIcon("Icon_TextColor_Main");
            Underscore = (Path)ResourceHelper.GetIcon("Icon_TextColor_Underscore");
            container.Children.Add(icon);
            container.Children.Add(Underscore);

            scaler.Child = container;

            return scaler;
        }

        protected override void UseCurrentColor(IBrowserControl browserControl)
        {
            var color = ColorPicker.LastSelectedColor;
            browserControl.ExecuteCommand("ForeColor", false, Utils.FormatColor(color));
        }
    }
}
