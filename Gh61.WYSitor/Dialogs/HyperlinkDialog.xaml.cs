namespace Gh61.WYSitor.Dialogs
{
    /// <summary>
    /// Interaction logic for HyperlinkDialog.xaml
    /// </summary>
    public partial class HyperlinkDialog : DialogBase
    {
        private string _url;
        private string _text;

        public HyperlinkDialog(string text = null, string url = "http://")
        {
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
    }
}
