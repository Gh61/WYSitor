using System.Globalization;

namespace Gh61.WYSitor.Localization
{
    /// <summary>
    /// Interface, you need to implement, if you want to provide custom localization for WYSitor.
    /// WYSitor will load all .dll files named "Gh61.WYSitor.Locale.*.dll" and will try to use it's IResourceManager.
    /// Implementation of this interface needs to have parameter-less constructor.
    /// </summary>
    /// <remarks>
    /// You can see class <see cref="DefaultResources"/> for names and default values of localizable texts.
    /// </remarks>
    public interface IResourceManager
    {
        /// <summary>
        /// Will search for localization of text under the given <see cref="textKey"/>.
        /// Returns <c>true</c>, when the localization is successful and localized text is in <see cref="localizedText"/>.
        /// Returns <c>false</c>, when there is no localization for given key and <see cref="culture"/>.
        /// </summary>
        /// <param name="textKey">The key to the specific text.</param>
        /// <param name="culture">Culture of returned localized text.</param>
        /// <param name="localizedText">Returned localized text. Empty text is treated like failed localization (the same way as returning <c>false</c>).</param>
        /// <returns>Whether the localization was successful.</returns>
        bool TryProvideString(string textKey, CultureInfo culture, out string localizedText);
    }
}
