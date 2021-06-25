using System.Windows;
using System.Windows.Controls;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Element, that can be used as Separator.
    /// </summary>
    public class ToolbarSeparatorElement: ToolbarElement
    {
        /// <summary>
        /// You can use simply this constructor. No need for identifier or anything.
        /// </summary>
        public ToolbarSeparatorElement() : base("Separator" + ++_separatorCount)
        { }

        protected override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            return new Separator();
        }

        private static int _separatorCount;
    }
}
