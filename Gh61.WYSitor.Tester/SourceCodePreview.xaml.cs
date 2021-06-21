using System.Windows;

namespace Gh61.WYSitor.Tester
{
    /// <summary>
    /// Interaction logic for SourceCodePreview.xaml
    /// </summary>
    public partial class SourceCodePreview : Window
    {
        public SourceCodePreview(TestViewModel vm)
        {
            InitializeComponent();

            this.DataContext = vm;
        }

        private void RestoreDefaultContent(object sender, RoutedEventArgs e)
        {
            ((TestViewModel) DataContext).RestoreDefaultContent();
        }
    }
}
