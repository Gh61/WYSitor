using System.Windows.Media;
using Gh61.WYSitor.Html;
using Gh61.WYSitor.ViewModels;
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
        /// Returns current html content of this editor.
        /// </summary>
        string GetCurrentHtml();

        /// <summary>
        /// Returns current selected text (or just current position of cursor).
        /// </summary>
        SelectedRange GetSelectedRange();

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

        /// <summary>
        /// Gets if this editor is in source edit mode.
        /// (You can change mode with <see cref="ToggleSourceEditor"/>.)
        /// </summary>
        bool IsInSourceEditMode { get; }

        /// <summary>
        /// Will change switch between WYSIWYG and HTML code editor.
        /// </summary>
        /// <param name="model">Instance of ToolbarViewModel.</param>
        /// <param name="enableSourceEditor">True for HTML editor, False for WYSIWYG editor.</param>
        void ToggleSourceEditor(ToolbarViewModel model, bool enableSourceEditor);
    }
}
