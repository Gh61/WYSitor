using System.Windows;
using System.Windows.Controls;
using Gh61.WYSitor.Html;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    internal sealed class FontSizePicker : ToolbarElement
    {
        public FontSizePicker()  : base(nameof(StandardToolbarElement.FontSize))
        {
            EnableCheckState = true;
        }

        protected override void CheckState(FrameworkElement element, IBrowserControl browserControl)
        {
            var currentSize = GetCurrentFontSize(browserControl);
            if (currentSize != null)
            {
                ((ComboBox)element).SelectedItem = currentSize;
            }
        }

        protected override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            /*
             * <ComboBox Width="42">
                    <ComboBox.ItemTemplate>
                        <DataTemplate>
                            <TextBlock Text="{Binding Text}" />
                        </DataTemplate>
                    </ComboBox.ItemTemplate>
                </ComboBox>
            */

            var comboBox = new ComboBox();
            comboBox.Width = 42;

            comboBox.ItemsSource = FontSize.AllFontSizes;
            comboBox.DisplayMemberPath = nameof(FontSize.Text);
            comboBox.SelectionChanged += (s, e) =>
            {
                FontSizeChanged(browserControl, comboBox);
            };

            return comboBox;
        }

        private static void FontSizeChanged(IBrowserControl browserControl, ComboBox comboBox)
        {
            var currentSize = GetCurrentFontSize(browserControl);
            var selectedSize = (FontSize)comboBox.SelectedValue;

            // only when change occurred
            if (selectedSize != currentSize)
            {
                browserControl.SetFontSize(selectedSize);
            }
        }

        private static FontSize GetCurrentFontSize(IBrowserControl browserControl)
        {
            string size = browserControl.CurrentDocument.queryCommandValue("FontSize")?.ToString();

            if (!int.TryParse(size, out var sizeKey))
                return null;

            if (!FontSize.SizeByKey.TryGetValue(sizeKey, out var fontSize))
                return null;

            return fontSize;
        }
    }
}
