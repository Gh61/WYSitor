using System;
using mshtml;

namespace Gh61.WYSitor.Html
{
    /// <summary>
    /// Selected text range.
    /// </summary>
    public class SelectedRange
    {
        private readonly IHTMLTxtRange _range;

        internal SelectedRange(IHTMLTxtRange range)
        {
            _range = range ?? throw new ArgumentNullException(nameof(range));
        }

        private SelectedRange(){}

        /// <summary>
        /// Gets text representation of this range.
        /// </summary>
        public string Text => _range?.text;

        /// <summary>
        /// Gets HTML representation of selected text.
        /// </summary>
        public string HtmlText => _range?.htmlText;

        /// <summary>
        /// Will paste html over this selection (or on this cursor position).
        /// </summary>
        public void PasteHtml(string html) => _range?.pasteHTML(html);

        /// <summary>
        /// Will select (again) this range.
        /// </summary>
        public void Select() => _range?.select();

        /// <summary>
        /// Gets whether this selection is empty (nothing is selected).
        /// </summary>
        public bool IsEmpty => string.IsNullOrEmpty(Text);

        private static readonly Lazy<SelectedRange> EmptyLazy = new Lazy<SelectedRange>(() => new SelectedRange());
        /// <summary>
        /// Gets Selected range, empty object.
        /// </summary>
        public static SelectedRange Empty => EmptyLazy.Value;
    }
}
