using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gh61.WYSitor.Code;

namespace Gh61.WYSitor.Dialogs
{
    /// <summary>
    /// Interaction logic for HyperlinkDialog.xaml
    /// </summary>
    public partial class HyperlinkDialog : Window, INotifyPropertyChanged
    {
        private string _url;
        private string _text;

        public HyperlinkDialog(string text = null, string url = "http://")
        {
            this.HideIcon();
            InitializeComponent();

            _text = text;
            _url = url;
            this.DataContext = this;
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public string Url
        {
            get => _url;
            set
            {
                if (_url == value) return;
                _url = value;
                OnPropertyChanged();
            }
        }

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void OkClicked(object sender, RoutedEventArgs e) => DialogResult = true;

        private void CancelClicked(object sender, RoutedEventArgs e) => DialogResult = false;

        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAllText();
            }
        }

        private void LabelClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label && label.Target != null)
            {
                label.Target.Focus();
            }
        }
    }
}
