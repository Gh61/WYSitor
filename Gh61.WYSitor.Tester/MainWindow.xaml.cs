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

            this.DataContext = new TestViewModel();

            // Testing API
            AddCustomButtons();
            AddPreviewButton();
        }

        private ObservableCollection<ToolbarElement> ToolbarItems => HtmlEditor.Toolbar.ToolbarElements;

        private void AddPreviewButton()
        {
            SourceCodePreview preview = null;
            var previewButton = new ToolbarButton(
                "SourceCodePreview",
                "Source code Preview",
                new Bold(new Run("HTML")),
                b =>
                {
                    if (preview == null)
                    {
                        preview = new SourceCodePreview((TestViewModel)this.DataContext);
                        preview.Closed += (s, e) =>
                        {
                            preview = null;
                        };
                    }
                    preview.Show();
                    preview.Focus();
                });
            previewButton.DisableEditorFocusAfterClick = true;

            ToolbarItems.Add(previewButton);
        }

        private void AddCustomButtons()
        {
            ToolbarButton customButton = null;
            customButton = new ToolbarButton(
                "MyCustomButton",
                "Try me!",
                new Bold(new Run("TRY ME!") { FontFamily = new FontFamily("Times New Roman") }),
                b =>
                {
                    MessageBox.Show("You clicked my custom button.\r\nEditing toolbar...", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);

                    AddMoreButtons();
                    RemoveUnderlineButton();
                    SwitchBoldAndItalic();
                    ReplaceSeparator();

                    ToolbarItems.Remove(customButton);
                });

            // add button to last place
            ToolbarItems.Add(customButton);

            // insert separator before button
            var position = ToolbarItems.Count - 1;
            ToolbarItems.Insert(position, new ToolbarSeparatorElement());
        }

        private void AddMoreButtons()
        {
            var signatureButton = new ToolbarButton(
                "Signature",
                "Signature",
                ResourceHelper.GetIcon("Icon_Signature"),
                b =>
                {
                    b.GetSelectedRange().PasteHtml("[SIGNATURE]");
                });
            var restoreButton = new ToolbarButton(
                "RestoreButton",
                "Restores whole toolbar to default",
                new Run("RESTORE"),
                browser =>
                {
                    ToolbarCommands.RestoreAll(HtmlEditor.Toolbar);
                });
            var resetButton = new ToolbarButton(
                "ResetButton",
                "Resets the editor",
                new Run("RESET"),
                browser =>
                {
                    HtmlEditor.Browser.OpenDocument();
                });

            ToolbarItems.Add(new ToolbarSeparatorElement());
            ToolbarItems.Add(restoreButton);
            ToolbarItems.Add(resetButton);
            ToolbarItems.Add(signatureButton);
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
            ToolbarItems[sepIndex] = new ToolbarButton("HA", "Custom Separator", new Run("|"), c => { });
        }
    }
}
