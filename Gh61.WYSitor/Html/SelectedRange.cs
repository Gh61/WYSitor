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
            _range = range;
        }

        public string Text => _range.text;

        public string HtmlText => _range.htmlText;

        public void PasteHtml(string html) => _range.pasteHTML(html);
    }
}
