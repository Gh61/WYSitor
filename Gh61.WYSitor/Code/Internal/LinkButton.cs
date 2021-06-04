using System.Windows;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Properties;

namespace Gh61.WYSitor.Code
{
    internal class LinkButton : ToolbarButton
    {
        public LinkButton() : base("Link", Resources.Text_Link, ResourceHelper.GetIcon("Icon_Earth"), null)
        {
        }

        public override void Clicked(IBrowserControl browserControl)
        {
            MessageBox.Show("TODO: Open dialog box");
        }
    }
}
