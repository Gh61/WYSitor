using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;
using MessageBox = System.Windows.MessageBox;

namespace Gh61.WYSitor.Code
{
    internal class HighlightColorButton : ToolbarSplitButton
    {
        public HighlightColorButton() : base("HighlightColor", Resources.Text_HighlightColor)
        {
        }

        protected override UIElement CreateMainButtonContent()
        {
            /*
             * <Viewbox Height="16">
                    <Grid>
                        <ContentControl Content="{StaticResource Icon_HighlightColor_Main}" Margin="-2,0,2,0" />
                        <ContentControl Content="{StaticResource Icon_HighlightColor_Underscore}" Margin="-2,0,2,0" />
                    </Grid>
                </Viewbox>
             */

            var scaler = new Viewbox();
            scaler.Height = 16;

            var container = new Grid();
            var icon = ResourceHelper.GetIcon("Icon_HighlightColor_Main");
            Underscore = (Path)ResourceHelper.GetIcon("Icon_HighlightColor_Underscore");
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
