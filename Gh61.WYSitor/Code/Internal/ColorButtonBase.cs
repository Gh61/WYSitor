using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    internal abstract class ColorButtonBase : ToolbarSplitButton
    {
        private readonly Color _defaultColor;
        private Path _underscore;

        protected ColorButtonBase(string identifier, string name, Color defaultColor) : base(identifier, name)
        {
            _defaultColor = defaultColor;
        }

        /// <summary>
        /// Gets or sets the underscore path, which will change color depending on current settings.
        /// </summary>
        protected Path Underscore
        {
            get => _underscore;
            set
            {
                _underscore = value;
                if (_underscore != null)
                {
                    _underscore.Fill = ColorPicker?.LastSelectedBrush ?? new SolidColorBrush(_defaultColor);
                }
            }
        }

        /// <summary>
        /// Gets current Color picker control.
        /// </summary>
        protected ColorPickerControl ColorPicker { get; private set; }

        /// <summary>
        /// Action fired after selected color might be used.
        /// </summary>
        protected abstract void UseCurrentColor(IBrowserControl browserControl);

        protected override void MainButtonClicked(IBrowserControl browserControl)
        {
            UseCurrentColor(browserControl);
        }

        protected override ContextMenu CreateContextMenu(IBrowserControl browserControl)
        {
            ColorPicker = new ColorPickerControl(ColorPicker?.LastSelectedColor ?? _defaultColor);
            ColorPicker.ColorSelected += (s, e) =>
            {
                if (Underscore != null)
                    Underscore.Fill = e.SelectedBrush;

                UseCurrentColor(browserControl);
                browserControl.Focus(); // focus back to editor
            };

            return ColorPicker.CreateContextMenu();
        }
    }
}
