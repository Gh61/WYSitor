using System.Globalization;
using Gh61.WYSitor.Localization;

namespace Gh61.WYSitor.Tester
{
    internal class MyResources : IResourceManager
    {
        public bool TryProvideString(string textKey, CultureInfo culture, out string localizedText)
        {
            localizedText = null;

            if (textKey == nameof(DefaultResources.Text_Image))
            {
                localizedText = "I said Image !!!";
                return true;
            }

            return false;
        }
    }
}
