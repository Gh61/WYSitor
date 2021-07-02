using System.Windows;
using Gh61.WYSitor.Dialogs;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Localization;
using Gh61.WYSitor.Views;
using mshtml;

namespace Gh61.WYSitor.Code
{
    internal class LinkButton : ToolbarButton
    {
        public LinkButton() : base(nameof(StandardToolbarElement.Link), ResourceManager.Text_Link, ResourceHelper.GetIcon("Icon_Earth"), null)
        {
        }

        public override void Clicked(IBrowserControl browserControl)
        {
            var range = (IHTMLTxtRange)browserControl.CurrentDocument.selection.createRange();
            var selectedText = range.text;

            var linkDialog = new HyperlinkDialog(selectedText);
            linkDialog.Owner = Window.GetWindow((EditorBrowser)browserControl);
            if (linkDialog.ShowDialog() == true)
            {
                CreateAnchor(range, linkDialog);
            }
        }

        private void CreateAnchor(IHTMLTxtRange selectedRange, HyperlinkDialog resultDialog)
        {
            var url = resultDialog.Url.EscapeHtml();
            var txt = resultDialog.Text.EscapeHtml(true);
            if (string.IsNullOrEmpty(txt))
            {
                txt = resultDialog.Url.EscapeHtml(true);
            }
            string anchor = $"<a href=\"{url}\">{txt}</a>";
            selectedRange.pasteHTML(anchor);
        }
    }
}
