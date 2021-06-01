using System.Windows;
using System.Windows.Controls;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Element, that can be used as Separator
    /// </summary>
    public class ToolbarSeparatorElement: ToolbarElement
    {
        public ToolbarSeparatorElement() : base("Separator" + ++_separatorCount)
        { }

        public override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            return new Separator();
        }

        private static int _separatorCount;
    }
}
