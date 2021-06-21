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
        /// <returns>Returns if content has changed.</returns>
        bool OpenDocument(string fileContent = null);

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
        /// This is shortcut for CurrentDocument.execCommand, but can be called without reference to mshtml.
        /// </remarks>
        void ExecuteCommand(string commandId, bool showUI = false, object value = null);

        /// <summary>
        /// Checks state of given command by its ID.
        /// </summary>
        /// <remarks>
        /// This is shortcut for CurrentDocument.queryCommandState, but can be called without reference to mshtml.
        /// </remarks>
        object QueryCommandState(string commandId);

        /// <summary>
        /// Checks if given command is enabled by its ID.
        /// </summary>
        /// <remarks>
        /// This is shortcut for CurrentDocument.queryCommandEnabled, but can be called without reference to mshtml.
        /// </remarks>
        bool QueryCommandEnabled(string commandId);

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
        /// Gets or sets whether the print dialog (after Ctrl+P hotKeys) is enabled.
        /// </summary>
        bool EnablePrintHotKey { get; set; }

        /// <summary>
        /// Will change switch between WYSIWYG and HTML code editor.
        /// </summary>
        /// <param name="model">Instance of ToolbarViewModel.</param>
        /// <param name="enableSourceEditor">True for HTML editor, False for WYSIWYG editor.</param>
        void ToggleSourceEditor(ToolbarViewModel model, bool enableSourceEditor);
    }
}
