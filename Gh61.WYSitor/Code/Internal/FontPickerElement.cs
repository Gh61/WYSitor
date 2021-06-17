using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    internal sealed class FontPickerElement : ToolbarElement
    {
        public FontPickerElement() : base("Font")
        {
            EnableCheckState = true;
        }

        public override void CheckState(FrameworkElement element, IBrowserControl browserControl)
        {
            var currentFont = GetCurrentFont(browserControl);
            if (currentFont != null)
            {
                ((ComboBox) element).SelectedItem = currentFont;
            }
        }

        public override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            /*
             * <ComboBox Width="128">
                    <ComboBox.ItemContainerStyle>
                        <Style TargetType="{x:Type ComboBoxItem}">
                            <Setter Property="FontFamily"
                                    Value="{Binding Content, RelativeSource={RelativeSource Mode=Self}}" />
                            <Setter Property="FontSize"
                                    Value="14" />
                        </Style>
                    </ComboBox.ItemContainerStyle>
                </ComboBox>
             */

            var comboBox = new ComboBox();
            comboBox.Width = 128;

            var itemStyle = new Style(typeof(ComboBoxItem));
            itemStyle.Setters.Add(new Setter(Control.FontFamilyProperty, new Binding(".")));
            itemStyle.Setters.Add(new Setter(Control.FontSizeProperty, (double)14));
            comboBox.ItemContainerStyle = itemStyle;

            comboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source).ToList();
            comboBox.SelectionChanged += (s, e) =>
            {
                FontChanged(browserControl, comboBox);
            };

            return comboBox;
        }

        private static void FontChanged(IBrowserControl browserControl, ComboBox comboBox)
        {
            FontFamily currentFont = GetCurrentFont(browserControl);
            FontFamily selectedFont = (FontFamily)comboBox.SelectedValue;

            // only when change occurred
            if (!Equals(selectedFont, currentFont))
            {
                browserControl.SetFont(selectedFont);
            }
        }

        private static FontFamily GetCurrentFont(IBrowserControl browserControl)
        {
            string name = browserControl.CurrentDocument.queryCommandValue("FontName")?.ToString();
            if (name == null)
                return null;

            return new FontFamily(name);
        }
    }
}
