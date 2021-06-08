using System;
using System.Windows;

namespace Gh61.WYSitor.Code
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Resources/Icons.xaml
        /// </summary>
        public static Lazy<ResourceDictionary> Icons = new Lazy<ResourceDictionary>(() =>
        {
            var icons = new ResourceDictionary();
            icons.Source = new Uri("/Gh61.WYSitor;component/Resources/Icons.xaml", UriKind.RelativeOrAbsolute);
            return icons;
        });

        /// <summary>
        /// Resources/Styles.xaml
        /// </summary>
        internal static Lazy<ResourceDictionary> Styles = new Lazy<ResourceDictionary>(() =>
        {
            var icons = new ResourceDictionary();
            icons.Source = new Uri("/Gh61.WYSitor;component/Resources/Styles.xaml", UriKind.RelativeOrAbsolute);
            return icons;
        });

        /// <summary>
        /// Get resource from Resources/Icons.xaml
        /// </summary>
        public static UIElement GetIcon(string name)
        {
            return (UIElement)Icons.Value[name];
        }
    }
}
