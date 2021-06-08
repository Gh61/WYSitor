using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Gh61.WYSitor.Dialogs
{
    /// <summary>
    /// Interaction logic for ImageDialog.xaml
    /// </summary>
    public partial class ImageDialog : DialogBase
    {
        private OpenFileDialog _openFileDialog;
        private bool _isSuccess;
        private string _imagePath;
        private ImageSource _imageSource;

        public ImageDialog()
        {
            InitializeComponent();

            // Self DataContext
            DataContext = this;
        }

        /// <summary>
        /// Gets whether the image selection was successful.
        /// </summary>
        public bool IsSuccess
        {
            get => _isSuccess;
            private set
            {
                if (_isSuccess == value) return;
                _isSuccess = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            private set
            {
                if (_imagePath == value) return;
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            private set
            {
                if (_imageSource == value) return;
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            EnsureFileDialog();

            if (_openFileDialog.ShowDialog(this) == true)
            {
                ImagePath = _openFileDialog.FileName;
                LoadImage();
            }
        }

        private void LoadImage()
        {
            var uri = new Uri(ImagePath, UriKind.RelativeOrAbsolute);
            try
            {
                ImageSource = new BitmapImage(uri);
                IsSuccess = true;
            }
            catch
            {
                ImagePath = Properties.Resources.ImageForm_ErrorImage;
                IsSuccess = false;
                ImageSource = new BitmapImage(); // empty
            }
        }

        private void EnsureFileDialog()
        {
            if (_openFileDialog == null)
            {
                _openFileDialog = new OpenFileDialog();
                _openFileDialog.Title = this.Title;
                _openFileDialog.Filter = Properties.Resources.ImageForm_Filter + " (*.png, *.jpg, *.jpeg, *.bmp)|*.png;*.jpg;*.jpeg;*.bmp;";
            }
        }
    }
}
