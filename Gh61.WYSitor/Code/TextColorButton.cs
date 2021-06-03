using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;

namespace Gh61.WYSitor.Code
{
    public class TextColorButton : ToolbarSplitButton
    {
        public TextColorButton() : base("TextColor", Resources.Text_TextColor)
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

        private Path Underscore { get; set; }

        protected override void MainButtonClicked(IBrowserControl browserControl)
        {
            MessageBox.Show("Yes, you clicked");
        }

        protected override ContextMenu CreateContextMenu()
        {
            var contextMenu = new ContextMenu();

            var red = new MenuItem() {Header = "Red"};
            var blue = new MenuItem() {Header = "Blue"};
            red.Click += (s, e) => { Underscore.Fill = Brushes.Red; };
            blue.Click += (s, e) => { Underscore.Fill = Brushes.Blue; };

            contextMenu.Items.Add(red);
            contextMenu.Items.Add(blue);

            return contextMenu;
        }
    }
}
