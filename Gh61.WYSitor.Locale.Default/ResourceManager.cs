using System.Globalization;
using Gh61.WYSitor.Localization;

namespace Gh61.WYSitor.Locale.Default
{
    /// <summary>
    /// Resource manager for allowing Gh61.WYSitor to localize it's texts.
    /// </summary>
    public class ResourceManager : IResourceManager
    {
        public bool TryProvideString(string textKey, CultureInfo culture, out string localizedText)
        {
            localizedText = Resources.ResourceManager.GetString(textKey, culture);
            return !string.IsNullOrEmpty(localizedText);
        }
    }
}
