using System.Collections.ObjectModel;
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
            SwitchBoldAndItalic();
            ReplaceSeparator();
        }

        private ObservableCollection<ToolbarElement> ToolbarItems => HtmlEditor.Toolbar.ToolbarElements;

        private void AddCustomButton()
        {
            var customButton = new ToolbarButton(
                "MyCustomButton",
                "Try me!",
                new Bold(new Run("TRY ME!") { FontFamily = new FontFamily("Times New Roman") }),
                b =>
                {
                    MessageBox.Show("You clicked my custom button.\r\nClearing toolbar... (and adding default B/I/U)", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);

                    ToolbarItems.Clear();

                    ToolbarItems.Add(ToolbarCommands.Bold);
                    ToolbarItems.Add(ToolbarCommands.Italic);
                    ToolbarItems.Add(ToolbarCommands.Underline);
                });

            // add button to last place
            ToolbarItems.Add(customButton);

            // insert separator before button
            var position = ToolbarItems.Count - 1;
            ToolbarItems.Insert(position, new ToolbarSeparatorElement());
        }

        private void RemoveUnderlineButton()
        {
            var underlineButton = ToolbarItems.FirstOrDefault(e => e.Identifier == "Underline");
            ToolbarItems.Remove(underlineButton);
        }

        private void SwitchBoldAndItalic()
        {
            var boldIndex = ToolbarItems.IndexOf(e => e.Identifier == "Bold");
            var italicIndex = ToolbarItems.IndexOf(e => e.Identifier == "Italic");

            ToolbarItems.Move(boldIndex, italicIndex);
        }

        private void ReplaceSeparator()
        {
            var sepIndex = ToolbarItems.IndexOf(e => e is ToolbarSeparatorElement);
            ToolbarItems[sepIndex] = new ToolbarButton("HA", "Separator", new Run("|||"), c => { });
        }
    }
}
