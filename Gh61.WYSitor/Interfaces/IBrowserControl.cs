using System.Windows.Media;
using Gh61.WYSitor.Html;
using mshtml;

namespace Gh61.WYSitor.Interfaces
{
    public interface IBrowserControl
    {
        /// <summary>
        /// Gets actually opened HTML document by internal browser.
        /// </summary>
        HTMLDocument CurrentDocument { get; }

        /// <summary>
        /// Loads given HTML or standard empty document.
        /// </summary>
        void OpenDocument(string fileContent = null);

        /// <summary>
        /// Executes given command on current document.
        /// </summary>
        /// <remarks>
        /// This is shortcut for CurrentDocument.execCommand, can be called without reference to mshtml.
        /// </remarks>
        void ExecuteCommand(string commandId, bool showUI = false, object value = null);

        /// <summary>
        /// Sets current font to the given one.
        /// </summary>
        void SetFont(FontFamily font);

        /// <summary>
        /// Sets current fontSize to given one.
        /// </summary>
        void SetFontSize(FontSize fontSize);

        /// <summary>
        /// Will try to set focus inside the editor.
        /// </summary>
        void Focus();
    }
}
