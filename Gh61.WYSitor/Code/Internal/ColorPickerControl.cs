using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Simple color palette.
    /// </summary>
    internal class ColorPickerControl : UserControl
    {
        private readonly Color[] _colors = 
        {
            Colors.White,
            Color.FromArgb(255, 255, 192, 192),
            Color.FromArgb(255, 255, 224, 192),
            Color.FromArgb(255, 255, 255, 192),
            Color.FromArgb(255, 192, 255, 192),
            Color.FromArgb(255, 192, 255, 255),
            Color.FromArgb(255, 192, 192, 255),
            Color.FromArgb(255, 255, 192, 255),
            Color.FromArgb(255, 224, 224, 224),
            Color.FromArgb(255, 255, 128, 128),
            Color.FromArgb(255, 255, 192, 128),
            Color.FromArgb(255, 255, 255, 128),
            Color.FromArgb(255, 128, 255, 128),
            Color.FromArgb(255, 128, 255, 255),
            Color.FromArgb(255, 128, 128, 255),
            Color.FromArgb(255, 255, 128, 255),
            Colors.Silver,
            Colors.Red,
            Color.FromArgb(255, 255, 128, 0),
            Colors.Yellow,
            Colors.Lime,
            Colors.Cyan,
            Colors.Blue,
            Colors.Fuchsia,
            Colors.Gray,
            Color.FromArgb(255, 192, 0, 0),
            Color.FromArgb(255, 192, 64, 0),
            Color.FromArgb(255, 192, 192, 0),
            Color.FromArgb(255, 0, 192, 0),
            Color.FromArgb(255, 0, 192, 192),
            Color.FromArgb(255, 0, 0, 192),
            Color.FromArgb(255, 192, 0, 192),
            Color.FromArgb(255, 64, 64, 64),
            Colors.Maroon,
            Color.FromArgb(255, 128, 64, 0),
            Colors.Olive,
            Colors.Green,
            Colors.Teal,
            Colors.Navy,
            Colors.Purple,
            Colors.Black,
            Color.FromArgb(255, 64, 0, 0),
            Color.FromArgb(255, 128, 64, 64),
            Color.FromArgb(255, 64, 64, 0),
            Color.FromArgb(255, 0, 64, 0),
            Color.FromArgb(255, 0, 64, 64),
            Color.FromArgb(255, 0, 0, 64),
            Color.FromArgb(255, 64, 0, 64),
        };

        private const int RowCount = 6;
        private const int ColumnCount = 8;
        private const int PreviewWidth = 20;
        private const int PreviewHeight = 20;
        private const int Spacing = 4;

        public ColorPickerControl() : this(Colors.Black)
        {
        }

        public ColorPickerControl(Color defaultColor)
        {
            InitializeComponents();
            LastSelectedColor = defaultColor;
            LastSelectedBrush = new SolidColorBrush(defaultColor);
        }

        /// <summary>
        /// Gets the last selected color.
        /// </summary>
        public Color LastSelectedColor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last selected brush.
        /// </summary>
        public Brush LastSelectedBrush
        {
            get;
            private set;
        }

        /// <summary>
        /// Event fired after some color is selected.
        /// </summary>
        public event EventHandler<SelectedColorArgs> ColorSelected;

        /// <summary>
        /// Creates content menu with this Color picker in it.
        /// </summary>
        public ContextMenu CreateContextMenu()
        {
            var contextMenu = new ContextMenu();
            //var menuItem = new MenuItem();
            //contextMenu.Items.Add(menuItem);

            // Using template to use ColorPicker inside ContextMenu
            contextMenu.Template = TemplateGenerator.CreateControlTemplate(typeof(ContextMenu), () =>
            {
                this.ColorSelected += (s, e) =>
                {
                    contextMenu.IsOpen = false;
                };

                var border = new Border();
                border.BorderBrush = SystemColors.ActiveBorderBrush;
                border.BorderThickness = new Thickness(1);
                border.Background = Brushes.WhiteSmoke;
                border.Child = this;
                return border;
            });

            return contextMenu;
        }

        private void InitializeComponents()
        {
            var canvas = new Canvas();

            int y = Spacing;
            for (int row = 0; row < RowCount; row++)
            {
                int x = Spacing;
                for (int column = 0; column < ColumnCount; column++)
                {
                    // create button with background
                    var button = new Button();
                    Canvas.SetLeft(button, x);
                    Canvas.SetTop(button, y);
                    button.Width = PreviewWidth;
                    button.Height = PreviewHeight;
                    var color = _colors[row * ColumnCount + column];
                    button.Background = new SolidColorBrush(color);
                    button.Tag = color;
                    button.Click += OnColorButtonClick;

                    // add to canvas
                    canvas.Children.Add(button);

                    x += PreviewWidth + Spacing;
                }
                y += PreviewHeight + Spacing;
            }

            // set canvas as this Content (and set size to proper display)
            Content = canvas;
            Width = ColumnCount * PreviewWidth + (ColumnCount + 1) * Spacing;
            Height = RowCount * PreviewHeight + (RowCount + 1) * Spacing;
        }

        private void OnColorButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var color = (Color)button.Tag;
            var brush = button.Background;

            LastSelectedColor = color;
            LastSelectedBrush = brush;

            ColorSelected?.Invoke(this, new SelectedColorArgs(color, brush));
        }

        /// <summary>
        /// Event args for selecting color.
        /// </summary>
        public class SelectedColorArgs : EventArgs
        {
            public SelectedColorArgs(Color selectedColor, Brush selectedBrush)
            {
                SelectedColor = selectedColor;
                SelectedBrush = selectedBrush;
            }

            public Color SelectedColor
            {
                get;
            }

            public Brush SelectedBrush
            {
                get;
            }
        }
    }
}
