using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Gh61.WYSitor.Code;

namespace Gh61.WYSitor.Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Testing API
            AddCustomButton();
            RemoveUnderlineButton();
        }

        private void AddCustomButton()
        {
            var customButton = new ToolbarButton(
                "MyCustomButton",
                "Try me!",
                new Bold(new Run("TRY ME!") { FontFamily = new FontFamily("Times New Roman") }),
                b =>
                {
                    MessageBox.Show("You clicked my custom button.", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                });

            // add button to last place
            HtmlEditor.Toolbar.ToolbarElements.Add(customButton);

            // insert separator before button
            var position = HtmlEditor.Toolbar.ToolbarElements.Count - 1;
            HtmlEditor.Toolbar.ToolbarElements.Insert(position, new ToolbarSeparatorElement());
        }

        private void RemoveUnderlineButton()
        {
            var underlineButton = HtmlEditor.Toolbar.ToolbarElements.FirstOrDefault(e => e.Identifier == "Underline");
            HtmlEditor.Toolbar.ToolbarElements.Remove(underlineButton);
        }
    }
}
