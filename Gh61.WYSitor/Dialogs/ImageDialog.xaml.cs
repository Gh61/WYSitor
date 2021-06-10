using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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
        private string _imageTitle;
        private double _imageWidth;
        private double _imageHeight;
        private int _reduceCoefficient;

        public ImageDialog()
        {
            InitializeComponent();

            // Self DataContext
            DataContext = this;
        }

        protected override void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            // Preview resize dependency
            switch (propertyName)
            {
                case nameof(ImageWidth):
                case nameof(ImageHeight):
                case nameof(ShowRealSize):
                    ResizePreview();
                    break;
            }
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

        /// <summary>
        /// Gets file path of selected image.
        /// </summary>
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

        public string ImageTitle
        {
            get => _imageTitle;
            set
            {
                if (_imageTitle == value) return;
                _imageTitle = value;
                OnPropertyChanged();
            }
        }

        #region Image size

        private int _originalWidth;
        private int _originalHeight;
        private bool _sizeIsChanging;
        private bool _showRealSize;

        /// <summary>
        /// Gets or sets final width of selected image.
        /// </summary>
        public double ImageWidth
        {
            get => _imageWidth;
            set
            {
                if (Math.Abs(_imageWidth - value) < 0.01) return;
                _imageWidth = value;
                RecalculateSize();
            }
        }

        /// <summary>
        /// Gets or sets final height of selected image.
        /// </summary>
        public double ImageHeight
        {
            get => _imageHeight;
            set
            {
                if (Math.Abs(_imageHeight - value) < 0.01) return;
                _imageHeight = value;
                RecalculateSize();
            }
        }

        /// <summary>
        /// Gets or sets coefficient, which is reducing size of the final image.
        /// Range: 1-100.
        /// </summary>
        public int ReduceCoefficient
        {
            get => _reduceCoefficient;
            set
            {
                if (value == _reduceCoefficient) return;
                _reduceCoefficient = value;
                RecalculateSize();
            }
        }

        private void RecalculateSize([CallerMemberName]string changedProperty = null)
        {
            if (_sizeIsChanging)
            {
                OnPropertyChanged(changedProperty);
                return;
            }

            try
            {
                _sizeIsChanging = true;

                double coef;
                switch (changedProperty)
                {
                    case nameof(ReduceCoefficient):
                        coef = ReduceCoefficient * 0.01;
                        ImageWidth = _originalWidth * coef;
                        ImageHeight = _originalHeight * coef;

                        break;

                    case nameof(ImageWidth):
                        coef = ImageWidth / _originalWidth;
                        if (coef > 1)
                        {
                            coef = 1;
                            ImageWidth = _originalWidth;
                        }

                        ImageHeight = Math.Round(_originalHeight * coef, 2);
                        ReduceCoefficient = (int)(coef * 100);
                        break;

                    case nameof(ImageHeight):
                        coef = ImageHeight / _originalHeight;
                        if (coef > 1)
                        {
                            coef = 1;
                            ImageHeight = _originalHeight;
                        }

                        ImageWidth = Math.Round(_originalWidth * coef, 2);
                        ReduceCoefficient = (int)(coef * 100);
                        break;
                }
            }
            finally
            {
                _sizeIsChanging = false;
            }
        }

        #region Preview

        public bool ShowRealSize
        {
            get => _showRealSize;
            set
            {
                if (value == _showRealSize) return;
                _showRealSize = value;
                OnPropertyChanged();
            }
        }

        private void ResizePreview()
        {
            var scrollView = (ScrollViewer)ImagePreview.Parent;

            if (ShowRealSize)
            {
                scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                ImagePreview.Width = ImageWidth;
                ImagePreview.Height = ImageHeight;
            }
            else
            {
                scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;

                ImagePreview.Width = scrollView.ActualWidth;
                ImagePreview.Height = scrollView.ActualHeight;
            }
        }

        #endregion

        #endregion

        #region Load image

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
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // so the image file wouldn't hang
                image.UriSource = new Uri(ImagePath, UriKind.Absolute);
                image.EndInit();

                ImageSource = image;
                IsSuccess = true;

                ImageTitle = Path.GetFileNameWithoutExtension(ImagePath);
                _imageWidth = _originalWidth = image.PixelWidth;
                _imageHeight = _originalHeight = image.PixelHeight;
                _reduceCoefficient = 100;
                OnPropertyChanged(nameof(ImageWidth));
                OnPropertyChanged(nameof(ImageHeight));
                OnPropertyChanged(nameof(ReduceCoefficient));
            }
            catch
            {
                IsSuccess = false;
                ImagePath = Properties.Resources.ImageForm_ErrorImage;
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

        #endregion
    }
}
